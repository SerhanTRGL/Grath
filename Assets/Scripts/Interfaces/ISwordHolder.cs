public interface ISwordHolder{
    public Sword Sword{get;set;}
    void AcquireSword(Sword swordToAcquire);
    void DropSword();
}
