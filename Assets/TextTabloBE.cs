using UnityEngine;

public class TextTabloBE : BehaviorExecutor
{
    public override void InitConstruct()
    {
         new RepeatSequance(this, false, out RepeatSequance rs, 9);
        
        
        
        new IntroPuttingTextNode(this, rs.current_count);
        new TimeOutNode(this, 1);




    }
}
