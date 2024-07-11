using UnityEngine;

namespace CodeBase._Services.Input
{
  public class MobileInputService : InputService
  {
    public override Vector2 Axis => SimpleInputAxis();
  }
}