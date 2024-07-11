using CodeBase.Logic.Ecs;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CodeBase.Logic.TouchInput
{
    public class UserTouchSystemForEditor : IProtoRunSystem
    {
        private readonly GameAspect _aspect;

        public UserTouchSystemForEditor(GameAspect aspect)
        {
            _aspect = aspect;
        }
        
        public void Run()
        {
            ref UserTouch touchComponent = ref GetInputComponent();
            if (touchComponent.IsOff)
            {
                RecordingTouchEndData(ref touchComponent);
                return;
            }
            
            if (EventSystem.current.IsPointerOverGameObject())
            {
                RecordingTouchEndData(ref touchComponent);
                return;
            }
            
            if (Input.GetMouseButtonDown(0))
            {
                RecordingStartTouchData(Input.mousePosition, ref touchComponent);
            }
            else if (Input.GetMouseButton(0))
            {
                if (touchComponent.HadStartPhase == false)
                    RecordingStartTouchData(Input.mousePosition, ref touchComponent);
                else
                    RecordingTouchMoveData(Input.mousePosition, ref touchComponent);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                RecordingTouchEndData(ref touchComponent);
            }
        }

        private ref UserTouch GetInputComponent()
        {
            var (entity, ok) = _aspect.TouchsIt.First();
            if (ok == false)
                return ref _aspect.Touchs.NewEntity();
            return ref _aspect.Touchs.Get(entity);;
        }
        
        private static void RecordingStartTouchData(Vector2 touchPosition, ref UserTouch touchComponent)
        {
            touchComponent.HadStartPhase = true;
            touchComponent.StartTime = Time.time;
            touchComponent.StartPosition = touchPosition;
            touchComponent.CurrentTime = Time.time;
            touchComponent.CurrentPosition = touchPosition;
            touchComponent.SwipeDelta = Vector2.zero;
            touchComponent.Delta = Vector2.zero;
            touchComponent.Direction = Vector2.zero;
            touchComponent.Duration = 0f;
            touchComponent.SqrDist = 0f;
            touchComponent.CurrentPhase = TouchPhase.Began;
        }

        private void RecordingTouchMoveData(Vector2 touchPosition, ref UserTouch touchComponent)
        {
            touchComponent.CurrentTime = Time.time;
            touchComponent.Duration = touchComponent.CurrentTime - touchComponent.StartTime;
            touchComponent.Delta = touchPosition - touchComponent.CurrentPosition ;
            touchComponent.CurrentPosition = touchPosition;
            touchComponent.Direction = touchComponent.Delta.normalized;
            touchComponent.SqrDist = touchComponent.Delta.SqrMagnitude();
            touchComponent.CurrentPhase = touchComponent.SqrDist >= 0 ? TouchPhase.Moved : TouchPhase.Stationary;
        }

        private void RecordingTouchEndData(ref UserTouch touchComponent)
        {
            touchComponent.SwipeDelta = touchComponent.CurrentPosition - touchComponent.StartPosition;
            touchComponent.HadStartPhase = false;
            touchComponent.CurrentPhase = TouchPhase.Ended;
        }
    }
}
