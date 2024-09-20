using System.Collections;
using UnityEngine;

public class ItemMagicScroll : IItemIteraction
{
    public string Spell;
    public override void Click()
    {
        if(Player.TryGetPlayer())
        Use(Player.me);
    }
    public static bool Equals(ItemMagicScroll A, ItemMagicScroll B)
    {
        return A.Spell == B.Spell;
    }
    public override IEnumerator RepeatUse()
    {
        yield break;
    }

    public override void Use(Element Creater)
    {
        MagIntegerField.mif.SetText(Spell);
        UseDestoy();
    }

    public override string ToStringInfo()
    {
        return
            "������� ����������: " +'"'+Spell+ '"'+"\n" +
            "�������� ��������� � ��������� ����������, � ��� �� ���� ��� � ������������, ��� � �����������.";

    }
}
