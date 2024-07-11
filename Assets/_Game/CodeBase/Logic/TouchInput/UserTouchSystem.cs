using CodeBase.Logic.Ecs;
using Leopotam.EcsProto;
using Leopotam.EcsProto.QoL;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CodeBase.Logic.TouchInput
{
    public class UserTouchSystem : IProtoRunSystem
    {
        private readonly GameAspect _aspect;

        public UserTouchSystem(GameAspect aspect)
        {
            _aspect = aspect;
        }
        
        public void Run()
        {
            if (Input.touchCount == 0)
                return;

            ref UserTouch touchComponent = ref GetInputComponent();
            if (touchComponent.IsOff)
            {
                RecordingTouchEndData(ref touchComponent);
                return;
            }

            Touch touch = Input.GetTouch(0);
            if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
            {
                RecordingTouchEndData(ref touchComponent);
                return;
            }

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    RecordingStartTouchData(touch, ref touchComponent);
                    break;
                case TouchPhase.Stationary:
                case TouchPhase.Moved:
                    if (touchComponent.HadStartPhase == false)
                        RecordingStartTouchData(touch, ref touchComponent);
                    else
                        RecordingTouchMoveData(touch, ref touchComponent);
                    break;
                case TouchPhase.Ended:
                    RecordingTouchEndData(ref touchComponent);
                    break;
            }
        }

        private ref UserTouch GetInputComponent()
        {
            var (entity, ok) = _aspect.TouchsIt.First();
            if (ok == false)
                return ref _aspect.Touchs.NewEntity();
            return ref _aspect.Touchs.Get(entity);;
        }

        private static void RecordingStartTouchData(Touch touch, ref UserTouch touchComponent)
        {
            touchComponent.HadStartPhase = true;
            touchComponent.StartTime = Time.time;
            touchComponent.StartPosition = touch.position;
            touchComponent.CurrentTime = Time.time;
            touchComponent.CurrentPosition = touch.position;
            touchComponent.CurrentPhase = TouchPhase.Began;
            touchComponent.SwipeDelta = Vector2.zero;
            touchComponent.Delta = Vector2.zero;
            touchComponent.Duration = 0f;
        }

        private void RecordingTouchMoveData(Touch touch, ref UserTouch touchComponent)
        {
            touchComponent.CurrentTime = Time.time;
            touchComponent.Delta = touch.position - touchComponent.CurrentPosition ;
            touchComponent.CurrentPosition = touch.position;
            touchComponent.CurrentPhase = touch.phase;
            touchComponent.Duration = touchComponent.CurrentTime - touchComponent.StartTime;
            touchComponent.Direction = touchComponent.Delta.normalized;
            touchComponent.SqrDist = touchComponent.Delta.SqrMagnitude();
        }
        
        private void RecordingTouchEndData(ref UserTouch touchComponent)
        {
            touchComponent.Delta = Vector2.zero;
            touchComponent.SwipeDelta = touchComponent.CurrentPosition - touchComponent.StartPosition;
            touchComponent.HadStartPhase = false;
            touchComponent.CurrentPhase = TouchPhase.Ended;
        }
    }
}
