// –‒–––‒––‒––––‒––––‒‒–––‒–‒‒––––‒–‒‒–‒––––‒‒‒–‒‒‒–––‒––‒‒––‒‒‒–––––‒‒–‒––
// Коммерческая лицензия подписчика
// (c) 2023 Leopotam <leopotam@yandex.ru>
// –‒–––‒––‒––––‒––––‒‒–––‒–‒‒––––‒–‒‒–‒––––‒‒‒–‒‒‒–––‒––‒‒––‒‒‒–––––‒‒–‒––

using UnityEngine.EventSystems;

namespace Leopotam.EcsProto.Unity.Ugui {
    public sealed class UnityUguiUpAction : UnityUguiAction<UnityUguiUpEvent>, IPointerUpHandler {
        public void OnPointerUp (PointerEventData eventData) {
            if (IsValidForEvent ()) {
                ref var msg = ref NewEvent ();
                msg.SenderName = SenderName ();
                msg.Sender = Sender ();
                msg.Position = eventData.position;
                msg.PointerId = eventData.pointerId;
                msg.Button = eventData.button;
            }
        }
    }
}
