using UnityEngine;

namespace CodeBase._Services.Input
{
  public interface IInputService : IService
  {
    Vector2 Axis { get; }

    bool IsAttackButtonUp();
  }
}