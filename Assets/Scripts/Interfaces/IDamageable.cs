using System.Collections;
public interface IDamageable{
    public int Health{get; set;}
    void TakeDamage(int damage);
    public float InvincibilityDuration{get; set;}
    IEnumerator MakeInvincible();
}
