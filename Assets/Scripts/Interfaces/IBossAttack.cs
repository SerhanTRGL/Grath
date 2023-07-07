using System.Collections;
using UnityEngine;

public interface IBossAttack{
    public IEnumerator Attack_State1();
    public IEnumerator Attack_State2();
    public IEnumerator Attack_State3();
    public IEnumerator Attack_State4();
}
