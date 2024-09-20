
using DynamicExpresso;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
public class ThElement
{
    public object value;

    public ThElement(object value)
    {
        this.value = value;
    }
}
public class MainSaverForThreading
{
    public ThElement posPlayer, posMy;
    public ThElement me, metodis, magic,
        deltatime, unscaledeltatime, mousedir, getlifetime;
    public bool updated = false;


    public void Update()
    {
        updated = true;

        posMy = new(((GameObject)me.value).transform.position);
        if (Player.TryGetPlayer())
            posPlayer = new(Player.me.transform.position);
        metodis = new(((Magic)magic.value).MeToDis());
        deltatime = new(Time.deltaTime);
        unscaledeltatime = new(Time.unscaledDeltaTime);
        mousedir = new(BaseFunc.GetPlayerFireDir());
        getlifetime = new(((Magic)magic.value).mattery.GetLifeTime());
    }
}
public abstract class Mattery : MonoBehaviour
{
    public MatteryEnergy matteryEnergy;
    public Energy energy;
    public BodyObject bodyObject;
    public TypeMattery typeMattery;
    public PhysicMove Phys;
    public Magic magic;
    public bool casting;
    public CurrentTriggerCollision ctc;
    public bool realize;
    public float starttime;
    public MagicPullet mypuller;
    public MagicPullet.Pullet pullet;
    public MainThreadCall mtc;
    public Element Creater;
    public SpellActivator.BoxLink UpdateSpeel;
    public Vector2 subdir;
    public static float offsettimeout,timeslowcast;
    public static Dictionary<string, Dictionary<string, Param>> MPA = new();
    public bool IsMSFT()
    {
        if (mtc.msft == null) return false;
        if (!mtc.msft.updated) return false;
        return true;
    }
    public string Speel;
    public abstract bool UseMagic(Element creater);


    public void Init()
    {
        matteryEnergy = new();
        starttime = Time.time;
        casting = false;
        realize = false;
        mtc.msft = new();
        magic = null;
        subdir = Vector2.zero;

    }
    public void InitThis(Energy energy)
    {
        this.energy = energy;
        typeMattery = TypeMattery.Energy;
        Init();
    }
    public void InitThis(BodyObject bodyObject)
    {
        this.bodyObject = bodyObject;
        typeMattery = TypeMattery.BodyObject;
        Init();
    }
    public float GetLifeTime()
    {
        if (magic.spellActivator.inited & casting)
        {
            return Time.time - starttime;
        }
        else return 0;
    }

}

[Serializable]
public class MatteryEnergy
{
    public float LiqEnergy;

}
[Serializable]
public enum ModifyParam
{
    Const,
    Dynamic
}
[Serializable]
public class Varible
{
    public Type type;
    public string name;
    public object value;

    public Varible(Type type, string name, object value)
    {
        this.type = type;
        this.name = name;
        this.value = value;
    }
}
public class Param
{
    public object value,startvalue;
    public string key;
    public ModifyParam modify;
    public string older_data;
    public PPR ppr;
    public Varible varible;
    public Param(object value, string key)
    {
        this.value = value;
        this.key = key;
        startvalue = value;
    }
    public static Param operator +(Param p, float f)
    {
        p.value = Convert.ToSingle(p.value) + f;
        return p;
    }
    public static Param operator *(Param p, float f)
    {
        p.value = Convert.ToSingle(p.value) * f;
        return p;
    }
    public static Param operator /(Param p, float f)
    {
        p.value = Convert.ToSingle(p.value) / f;
        return p;
    }
    public static Param operator -(Param p, float f)
    {
        p.value = Convert.ToSingle(p.value) - f;
        return p;
    }
    public void Recovery()
    {
        value = startvalue;
    }
    public Param(object value, string key, ModifyParam modify, string older_data)
    {
        this.value = value;
        startvalue = value;
        this.key = key;
        this.modify = modify;
        this.older_data = older_data;
    }
}
[Serializable]
public class CastInfo
{
    public TypeMagicCircle type;
    public float V, R, E;
    public Vector2 dir;
    public string Mod;
    public float currentimpulse;
    public void Update(CastParameter cast)
    {
        type = cast.type;
        V = Convert.ToSingle(cast.V.value);
        R = Convert.ToSingle(cast.R.value);
        E = Convert.ToSingle(cast.E.value);


        currentimpulse = cast.magic.spellActivator.GetImpulsMany();
        if (cast.Dir != null)
            dir = (Vector2)cast.Dir.value;
        Mod = cast.Mod;
    }
}
public class CastParameter
{
    public TypeMagicCircle type;
    public Param V, R, E, Dir;
    public string Mod;
    public bool IsDir;
    public Magic magic;
    public CastParameter(Magic magic)
    {
        this.magic = magic;
    }
    public CastParameter(CastParameter c)
    {
        type = c.type;
        V = c.V;
        R = c.R;
        E = c.E;
        Mod = c.Mod;
        Dir = c.Dir;
        magic = c.magic;
    }

}
[Serializable]
public class SpellActivator
{
    public Dictionary<string, Param> MoreParameters = new();
    public CastParameter castParameter;
    public List<Param> Dynamics = new();
    public Magic magic;
    public bool inited = false;
    public CastInfo castInfo = new(), startcastparameter = new();

    public Param AddParameter(object value, string key, ModifyParam modify, string older_data)
    {
        Param p = new(value, key, modify, older_data);
        if (key == null) return null;
        MoreParameters.Add(key, p);
        return p;
    }
    public void AddParameter(Param param)
    {
        if (GetParameter(param.key) == null)
        {
            MoreParameters.Add(param.key, param);
        }
        else
        {
            SetParameter(param.key, param.value, param.older_data);
        }

    }
    public static Thread main = Thread.CurrentThread;
    public void UpdateDynamic(object state)
    {
        while (((BoxLink)state).trfl)
        {



            if (!BaseFunc.Pause)
            {
                float d = 0.001f;
                if (magic.mattery.mtc.msft.updated)
                    d = Convert.ToSingle(magic.mattery.mtc.msft.deltatime.value);

                Thread.Sleep((int)(d * Mattery.offsettimeout * 1000));

                if (!magic.mattery.pullet.used) Thread.CurrentThread.Abort();
                // yield return new WaitForSeconds(Time.deltaTime * Mattery.offsettimeout);
                for (int i = 0; i < Dynamics.Count; i++)
                {
                    Thread.Sleep((int)(d * Mattery.offsettimeout * 1000)); //  yield return new WaitForSeconds(Time.deltaTime * Mattery.offsettimeout);
                    Magic.UpdateSpell(Dynamics[i].older_data, this, Dynamics[i]);
                }
            }
            else
            {
                for (; BaseFunc.Pause;)
                {
                    float d = 0.001f;
                    if (magic.mattery.mtc.msft.updated)
                        d = Convert.ToSingle(magic.mattery.mtc.msft.unscaledeltatime.value);
                    Thread.Sleep((int)(d * Mattery.offsettimeout * 1000));
                }
            }

        }
        Thread.CurrentThread.Abort();



    }
    public void InitCast()
    {

        magic.mattery.mtc.LocalAwake();
        magic.mattery.mtc.LocalUpdate();
        foreach (var item in MoreParameters)
        {

            switch (item.Key)
            {
                case "MI":
                    castParameter.type = (TypeMagicCircle)item.Value.value;
                    break;
                case "MOD":
                    castParameter.Mod = (string)item.Value.value;
                    break;
                case "V":
                    castParameter.V = item.Value;
                    break;
                case "R":
                    castParameter.R = item.Value;
                    break;
                case "E":
                    castParameter.E = item.Value;
                    break;
                case "DIR":
                    castParameter.Dir = item.Value;
                    castParameter.IsDir = true;
                    break;
                default:
                    switch (Magic.GetNameParam(item.Key))
                    {
                        case "FORM":
                            Magic.UpdateSpell(item.Value.older_data, this, item.Value);
                            continue;
                            break;
                    }


                    break;

            }
            if (item.Value.modify == ModifyParam.Dynamic) Dynamics.Add(item.Value);
        }
        inited = true;

        startcastparameter.Update(castParameter);

    }

    public float Fytem = 0;
    public bool update;
    public delegate void Us();
    public static void ToDelegate(Us us)
    {
        us();
    }
    public class BoxLink
    {
        public bool trfl;

        public BoxLink(bool trfl)
        {
            this.trfl = trfl;
        }
    }
    public ExceptionReadSpeel Use()
    {
        if (magic.mattery.casting)
            if (castParameter == null)
            {
                castParameter = new(magic);
                InitCast();
            }
            else
            {

                magic.mattery.mtc.LocalUpdate();
                if (magic.mattery.gameObject.activeSelf & !update)
                {
                    update = true;

                    WaitCallback wait = UpdateDynamic;
                    magic.mattery.UpdateSpeel = new(true);
                    ThreadPool.QueueUserWorkItem(wait, magic.mattery.UpdateSpeel);







                }

                if (Convert.ToSingle(castParameter.V.value) > 10)
                {
                    castParameter.R.value = 0f;
                }
                float E = Convert.ToSingle(castParameter.E.value),
                      V = Convert.ToSingle(castParameter.V.value),
                      R = Convert.ToSingle(castParameter.R.value);

                float F;
                magic.mattery.transform.localScale = Vector3.one * V;
                if (R == 0 || V == 0)
                {
                    F = 0;
                }
                else
                    F = Magic.GetCircleSpeed(castParameter.type) * E / (V * R);
                if (magic.matteryEnergy.LiqEnergy > 0)
                {
                    F += magic.matteryEnergy.LiqEnergy;
                    magic.matteryEnergy.LiqEnergy -= Time.deltaTime;

                }



                if (Convert.ToSingle(startcastparameter.E) < Convert.ToSingle(castParameter.E.value))
                {
                    castParameter.R /= 0.9f;
                    castParameter.V /= 0.9f;
                    castParameter.E *= 0.5f;
                    if (Convert.ToSingle(startcastparameter.E) + 1 < Convert.ToSingle(castParameter.E.value))
                    {
                        castParameter.E.value = 0f;
                    }

                    if (Mathf.Abs(Convert.ToSingle(castParameter.R.value)) > 100)
                    {
                        castParameter.E.value = 0f;


                    }
                }
                if (Convert.ToSingle(castParameter.E.value) <= 0)
                {
                    magic.mattery.mypuller.Import(magic.mattery.pullet.pos);
                    magic.mattery.UpdateSpeel.trfl = false;

                }
                if (magic.mattery.Phys.GetSpeed() > 25)
                {
                    castParameter.R *= 0.9f;
                    castParameter.V /= 0.9f;
                    castParameter.E *= 0.5f;
                    if (magic.mattery.Phys.GetSpeed() > 100)
                    {
                        castParameter.R *= 0.9f;
                        castParameter.V /= 0.9f;
                        castParameter.E *= 0.1f;
                    }

                }


                if (!castParameter.IsDir)
                {
                    if (!float.IsNaN(F))
                    {
                        magic.mattery.Phys.Move((magic.dir + magic.mattery.subdir).normalized * Mathf.Abs(F));
                    }
                    else castParameter.E.value = 0;
                }
                else
                {
                    Vector2 dir = (Vector2)castParameter.Dir.value;
                    magic.mattery.Phys.Move((dir.normalized + magic.mattery.subdir).normalized * Mathf.Abs(F));
                }
                castParameter.E -= Time.deltaTime * UnityEngine.Random.Range(1f, 10f) / 5f * (1 + Mathf.Abs(F))*Mattery.timeslowcast;
                castInfo.Update(castParameter);

            }

        return null;
    }
    public float GetImpulsMany()
    {
        return castInfo.E * (Mathf.Pow(1f - 1f / (1 + Mathf.Round(castInfo.V * castInfo.R)), 0.25f)) * Mathf.Sqrt(magic.mattery.Phys.GetSpeed()) / castInfo.V;
    }
    public Param GetParameter(string key)
    {
        if (MoreParameters.TryGetValue(key, out Param value))
        {
            return value;
        }
        else return null;

    }
    public bool TryGetParameter(string key, out Param param)
    {
        if (MoreParameters.TryGetValue(key, out Param value))
            if (value.value != null)
            {

                param = value;
                return true;
            }
        param = null;
        return false;
    }
    public void SetParameter(string key, object val, PPR ppr = null)
    {
        if (MoreParameters.TryGetValue(key, out Param value))
        {
            value.value = val;
            value.ppr = ppr;
        }


    }
    public SpellActivator(Magic magic)
    {
        this.magic = magic;
        inited = false;
        castParameter = null;
        Dynamics.Clear();
    }
    public void SetParameter(string key, object val, string older_data, PPR ppr = null)
    {
        if (MoreParameters.TryGetValue(key, out Param value))
        {
            value.value = val;
            value.older_data = older_data;
            value.ppr = ppr;
        }


    }
}
[Serializable]

public class Step
{
    public int i;
}
public class InfoCar
{
    public List<FormuLexer.Token> tokens;
    public Step i;
    public PPR ppr;
    public string sum = "", s = "";
    public string block;
    public List<Param> parameters;
    public Param me;
    public ModifyParam modify;
    public SpellActivator spellActivator;
    public Interpreter interpreter;
    public void SetInterpreter(Interpreter interpreter)
    {
        this.interpreter = interpreter;
    }
    //FuncBlock(string block, List<Param> parameters, out ModifyParam modify,SpellActivator spellActivator)
    //Block(string block, Param me, out ModifyParam modify, SpellActivator spellActivator)
    public InfoCar(List<FormuLexer.Token> tokens, Step i, PPR ppr, string sum, SpellActivator spellActivator, Interpreter interpreter)
    {
        this.tokens = tokens;
        this.ppr = ppr;
        this.sum = sum;
        this.ppr = ppr;
        this.i = i;
        this.interpreter = interpreter;
    }

    public InfoCar(string block, List<Param> parameters, out ModifyParam modify, SpellActivator spellActivator)
    {
        modify = ModifyParam.Const;
        this.block = block;
        this.parameters = parameters;
        this.modify = modify;
        this.spellActivator = spellActivator;
    }

    public InfoCar(List<FormuLexer.Token> tokens, Step i, PPR ppr, string sum, string s, SpellActivator spellActivator, Interpreter interpreter)
    {
        this.tokens = tokens;
        this.ppr = ppr;
        this.sum = sum;
        this.s = s;
        this.i = i;
        this.interpreter = interpreter;
    }

    public InfoCar(string block, Param me, out ModifyParam modify, SpellActivator spellActivator)
    {
        modify = ModifyParam.Const;
        this.block = block;
        this.me = me;
        this.modify = modify;
        this.spellActivator = spellActivator;
    }


}
[Serializable]
public class Magic
{

    public Vector2 dir;
    public SpellActivator spellActivator;
    public bool init;
    public MatteryEnergy matteryEnergy;
    public Mattery mattery;

    public bool IsReadSpelled;
    public float MeToDis()
    {
        if (mattery.Creater != null)
        {
            return Mathf.Round(Vector2.Distance(mattery.transform.position, mattery.Creater.transform.position) * 10000f) / 10000f;
        }
        else
        {
            return 0.01f;
        }
    }
    public static Vector2 RotationTo(Vector2 to_a)
    {
        Vector2 perpen, move_vector;


        perpen = (Quaternion.Euler(0, 0, 90) * to_a).normalized;
        //perpen = Vector2.Perpendicular((to_a*speed).normalized);
        move_vector = (perpen + to_a).normalized;
        return move_vector;
    }
    public static event Action PlayerThis;
    public static float GetCircleSpeed(TypeMagicCircle type)
    {
        return type switch
        {
            TypeMagicCircle.MIE => 1f,
            TypeMagicCircle.MIH => 0.417805974454175f,
            TypeMagicCircle.MIP => 0.239039035913821f,
            TypeMagicCircle.MIG => 0.156972294695809f,
            TypeMagicCircle.MIU => 0f,
            _ => float.NaN,
        };
    }
    public static object Block(InfoCar car, Interpreter interpreter)
    {
        car.modify = ModifyParam.Const;
        switch (car.block)
        {
            case "[TIMEGLOBAL]":
                if (car.me != null)
                {
                    car.me.modify = ModifyParam.Dynamic;
                }
                else
                    car.modify = ModifyParam.Dynamic;
                return Time.time;
            case "[TIME]":
                if (car.me != null)
                {
                    car.me.modify = ModifyParam.Dynamic;
                }
                else
                    car.modify = ModifyParam.Dynamic;
                if (car.spellActivator.magic.mattery.IsMSFT())
                {
                    return Convert.ToSingle(car.spellActivator.magic.mattery.mtc.msft.getlifetime.value) + 0.01f;
                }
                else
                {
                    return 0.01f;
                }

            case "[METODIS]":
                if (car.me != null)
                {
                    car.me.modify = ModifyParam.Dynamic;
                }
                else
                    car.modify = ModifyParam.Dynamic;
                if (car.spellActivator.magic.mattery.IsMSFT())
                {
                    return Convert.ToSingle(car.spellActivator.magic.mattery.mtc.msft.metodis.value) + 0.01f;
                }
                else
                {
                    return 0.01f;
                }

            case "[MOUSEDIR]":
                if (true)
                {
                    if (car.me != null)
                    {
                        car.me.modify = ModifyParam.Dynamic;
                    }
                    else
                        car.modify = ModifyParam.Dynamic;
                    Vector2 vector;
                    if (car.spellActivator.magic.mattery.IsMSFT())
                    {
                        vector = (Vector2)car.spellActivator.magic.mattery.mtc.msft.mousedir.value;
                    }
                    else
                    {
                        vector = Vector2.zero;
                    }
                    string res = FormuLexer.NumberToLetter(vector.x + vector.y + "");
                    interpreter.SetVariable(res, vector, vector.GetType());
                    car.ppr.type = vector.GetType();
                    car.ppr.varible = new(vector.GetType(), res, vector);
                    return res;
                }
            case "[PLAYERPOS]":
                if (true)
                {
                    if (car.me != null)
                    {
                        car.me.modify = ModifyParam.Dynamic;
                    }
                    else
                        car.modify = ModifyParam.Dynamic;
                    Vector2 vector = Vector2.zero;
                    if (Player.TryGetPlayer())
                    {

                        if (car.spellActivator.magic.mattery.IsMSFT())
                        {
                            vector = (Vector3)car.spellActivator.magic.mattery.mtc.msft.posPlayer.value;
                        }
                        else
                        {
                            vector = Vector2.zero;
                        }


                    }
                    string res = FormuLexer.NumberToLetter(vector.x + vector.y + "");
                    interpreter.SetVariable(res, vector, vector.GetType());
                    car.ppr.type = vector.GetType();
                    car.ppr.varible = new(vector.GetType(), res, vector);
                    return res;
                }
            case "[MEPOS]":
                if (true)
                {
                    if (car.me != null)
                    {
                        car.me.modify = ModifyParam.Dynamic;
                    }
                    else
                        car.modify = ModifyParam.Dynamic;
                    Vector2 vector = Vector2.zero;
                    if (car.spellActivator.magic.mattery.IsMSFT())
                    {
                        vector = (Vector3)car.spellActivator.magic.mattery.mtc.msft.posMy.value;
                    }
                    else
                    {
                        vector = Vector2.zero;
                    }


                    string res = FormuLexer.NumberToLetter(vector.x + vector.y + "");
                    interpreter.SetVariable(res, vector, vector.GetType());
                    car.ppr.type = vector.GetType();
                    car.ppr.varible = new(vector.GetType(), res, vector);
                    return res;
                }

            default:
                return new ExceptionReadSpeel("Неизвестный спейсер");
        }
    }


    public static object FuncBlock(InfoCar car, Interpreter interpreter)
    {
        car.modify = ModifyParam.Const;
        switch (car.block)
        {
            case "SIN":
                if (car.parameters.Count >= 1)
                {


                    FormuLexer lexer = new(car.parameters[0].value.ToString(), car.spellActivator);

                    car.modify = car.parameters[0].modify;
                    float result = Mathf.Sin(Convert.ToSingle(lexer.Parse()));
                    if (result == 0) result = 0.01f;
                    return result;

                }
                return new ExceptionReadSpeel("неизвестный тип параметра/параметров слишком мало");
            case "PERPENVECTOR":
                if (true)
                {
                    if (car.me != null)
                    {
                        car.me.modify = ModifyParam.Dynamic;
                    }
                    else
                        car.modify = ModifyParam.Dynamic;

                    Vector2 vector;
                    FormuLexer lexer = new(car.parameters[0].value.ToString(), car.spellActivator);

                    car.modify = car.parameters[0].modify;
                    lexer.interpreter = interpreter;
                    lexer.Localppr = car.parameters[0].ppr;
                    vector = (Vector2)lexer.Parse();
                    vector = RotationTo(vector);
                    string res = FormuLexer.NumberToLetter(vector.x + vector.y + "");
                    interpreter.SetVariable(res, vector, vector.GetType());
                    car.ppr.type = vector.GetType();
                    car.ppr.varible = new(vector.GetType(), res, vector);
                    return res;
                }

            default:
                return new ExceptionReadSpeel("Неизвестный фанк-спейсер");
        }

    }
    public static bool IsOperated(char s)
    {
        return s == '+' || s == '-' || s == '*' || s == '/';
    }
    public static Param CyrliFormule(string cur, SpellActivator spellActivator)
    {

        string key = "", formula = "";
        bool ky = false;
        ModifyParam modify;





        for (int i = 0; i < cur.Length; i++)
        {
            if (!ky)
            {
                if (cur[i] != '=')
                {
                    key += cur[i];
                }
                else
                {
                    ky = true;
                }
            }
            else
            {
                formula += cur[i];

            }

        }
        Param p = spellActivator.GetParameter(key);

        FormuLexer lexer = new(formula, spellActivator);

        object result = lexer.Parse();


        // Param pr = spellActivator.GetParameter(param);
        //result = Convert.ToSingle(Block(block, spellActivator.GetParameter(key));

        //result = Convert.ToSingle(FuncBlock(block, CyrliListParameters(func), spellActivator.GetParameter(key));




        if (p == null) return null;
        p.value = result;
        p.ppr = lexer.Localppr;
        p.varible = lexer.Localppr.varible;
        p.modify = lexer.Localppr.modify;
        p.older_data = "FORM(" + cur + ")";
        return p;

    }
    /*
    public string PointToComme(string s)
    {
        string sum = "";
        for(int i = 0; i < s.Length; i++)
        {
            if (s[i] == '.')
            {

            }else
        }
    }
    */
    public static Param CyrliListParameters(string cur, SpellActivator spellActivator)
    {
        List<Param> parameters = new();
        string block = "", func = "";
        bool bl = false, funcbl = false, funcpar = false;
        string sum = "";

        for (int i = 0; i < cur.Length; i++)
        {
            if (cur[i] != ',')
            {
                sum += cur[i];
            }
            else
            {
                FormuLexer lexer = new(sum, spellActivator);
                Param lcl1 = new(lexer.ToTextParse(), sum);
                lcl1.modify = lexer.Localppr.modify;
                lcl1.ppr = lexer.Localppr;
                parameters.Add(lcl1);
                sum = "";

            }
        }
        FormuLexer lexerr = new(sum, spellActivator);

        Param lcl = new(lexerr.ToTextParse(), sum);
        lcl.modify = lexerr.Localppr.modify;
        lcl.ppr = lexerr.Localppr;
        lcl.varible = lexerr.Localppr.varible;
        parameters.Add(lcl);
        Param p = new(parameters, "list");
        return p;

    }
    public static char FutureSymbol(int index, string text, int move = 1)
    {
        if (index + move < text.Length)
        {
            return text[index + move];
        }
        else return '\0';
    }
    public static Param Pareth(string sumcond, SpellActivator spellActivator)
    {
        string key = "", sum = "";
        bool pareth = false;
        ModifyParam modify = ModifyParam.Const;
        for (int i = 0; i < sumcond.Length; i++)
        {
            if (sumcond[i] != '(' & sumcond[i] != ')' & !pareth)
            {

                key += sumcond[i];
            }
            else
            {
                if (sumcond[i] == '(')
                {
                    pareth = true;
                }
                sum += sumcond[i];


            }
        }
        FormuLexer lexer = new(sum, spellActivator);

        object res = lexer.ToTextParse();
        modify = lexer.Localppr.modify;
        if (res == null) return null;
        if (PointFormat.TryParse(res.ToString(), out float result))
        {

            return new(result, key, modify, sumcond);
        }
        else
        {
            if (res.GetType().Name == "Vector2")
            {
                return new(res, key, modify, sumcond);
            }
            return null;
        }
    }
    public static string GetNameParam(string sum)
    {
        string nam = "";
        for (int i = 0; i < sum.Length; i++)
        {
            if (sum[i] == '_') break;
            if (sum[i] != '(' & sum[i] != '{' & sum[i] != '[')
            {
                nam += sum[i];
            }
            else return nam;
        }
        return nam;
    }
    public static string GetPathArgument(string sum)
    {
        string arg = "";
        char startsymbol = '\0';
        int count = 0;
        for (int i = 0; i < sum.Length; i++)
        {
            if (startsymbol == '\0')
            {
                if (sum[i] == '{' || sum[i] == '(' || sum[i] == '[')
                {
                    startsymbol = sum[i];
                }
            }
            else
            {


                if ((sum[i] == '}' & startsymbol == '{') || (sum[i] == ')' & startsymbol == '(') || (sum[i] == ']' & startsymbol == '['))
                {
                    if (count == 0)
                    {
                        return arg;
                    }
                    else
                        count--;
                    arg += sum[i];
                }
                else
                {
                    if (sum[i] == startsymbol)
                    {
                        count++;
                        arg += sum[i];
                    }
                    else
                    {
                        arg += sum[i];
                    }
                }

            }
        }
        return arg;

    }
    public static List<string> GetListPathArgument(string sum)
    {
        List<string> list = new();
        string arg = "";
        char startsymbol = '\0';
        int count = 0;
        for (int i = 0; i < sum.Length; i++)
        {
            if (startsymbol == '\0')
            {
                if (sum[i] == '{' || sum[i] == '(' || sum[i] == '[')
                {
                    startsymbol = sum[i];
                }
            }
            else
            {


                if ((sum[i] == '}' & startsymbol == '{') || (sum[i] == ')' & startsymbol == '(') || (sum[i] == ']' & startsymbol == '['))
                {
                    if (count == 0)
                    {
                        list.Add(arg);
                    }
                    else
                        count--;
                    arg += sum[i];
                }
                else
                {
                    if (sum[i] == startsymbol)
                    {
                        count++;
                        arg += sum[i];
                    }
                    else
                    {
                        arg += sum[i];
                    }
                }

            }
        }
        if (arg != "")
            list.Add(arg);
        return list;

    }
    public static ExceptionReadSpeel UpdateSpell(string sum, SpellActivator spellActivator, Param modP)
    {
        switch (sum)
        {
            case "MIE":
                spellActivator.AddParameter(TypeMagicCircle.MIE, "MI", ModifyParam.Const, sum);
                break;
            case "O":
                spellActivator.AddParameter(sum, "MOD", ModifyParam.Const, sum);
                break;

            default:

                string nam = GetNameParam(sum);
                switch (nam)
                {
                    case "VAR":
                        break;
                        Param le = CyrliListParameters(GetPathArgument(sum), spellActivator);
                        List<Param> list = (List<Param>)le.value;
                        spellActivator.AddParameter(list, "VAR", le.modify, sum);
                        for (int x = 0; x < list.Count; x++)
                        {
                            spellActivator.AddParameter(null, (string)list[x].value, list[x].modify, sum);

                        }
                        return null;
                        break;
                    case "FORM":

                        Param param1 = CyrliFormule(GetPathArgument(sum), spellActivator);
                        if (param1 == null) return new("Ошибка формульных расчетов");
                        modP.modify = param1.modify;
                        return null;

                        break;
                }
                Param param = Pareth(sum, spellActivator);
                if (param != null)
                {
                    spellActivator.SetParameter(param.key, param.value);

                    return null;
                }
                return new ExceptionReadSpeel("Неизвестный параметр");
        }
        return null;
    }
    public ExceptionReadSpeel ReadSpeel(string speel)
    {
        List<string> conds = new();
        string sum = "";
        spellActivator = new(this);
        if (Mattery.MPA.TryGetValue(speel, out Dictionary<string, Param> more))
        {
            foreach (var par in more)
            {
                par.Value.Recovery();
                spellActivator.AddParameter(par.Value.value, par.Key, par.Value.modify, par.Value.older_data);
            }
           

        }
        else
        {
            for (int i = 0; i < speel.Length; i++)
            {
                if (speel[i] != ' ')
                {
                    sum += speel[i];
                }
                if ((speel[i] == ' ') || (i + 1 == speel.Length))
                {

                    switch (sum)
                    {
                        case "MIE":
                            spellActivator.AddParameter(TypeMagicCircle.MIE, "MI", ModifyParam.Const, sum);
                            break;
                        case "O":
                            spellActivator.AddParameter(sum, "MOD", ModifyParam.Const, sum);
                            break;

                        default:

                            string nam = GetNameParam(sum);
                            switch (nam)
                            {
                                case "VAR":

                                    List<Param> list = (List<Param>)CyrliListParameters(GetPathArgument(sum), spellActivator).value;
                                    spellActivator.AddParameter(list, "VAR", ModifyParam.Const, sum);
                                    for (int x = 0; x < list.Count; x++)
                                    {
                                        spellActivator.AddParameter(null, (string)list[x].value, list[x].modify, sum);

                                    }
                                    sum = "";
                                    continue;
                                    break;
                                case "FORM":


                                    UpdateSpell(sum, spellActivator, spellActivator.AddParameter(GetPathArgument(sum), nam + "_" + i, ModifyParam.Const, sum));
                                    sum = "";
                                    // CyrliFormule(GetPathArgument(sum),spellActivator);
                                    continue;

                                    break;
                            }
                            Param param = Pareth(sum, spellActivator);
                            if (param != null)
                            {

                                spellActivator.AddParameter(param);
                                sum = "";
                                continue;
                                break;
                            }
                            return new ExceptionReadSpeel("Неизвестный параметр");
                    }
                    sum = "";
                }

            }
            Mattery.MPA.Add(speel, spellActivator.MoreParameters);
        }
        IsReadSpelled = true;
        return null;
    }
    public Magic(MatteryEnergy matteryEnergy, Mattery mattery, Vector2 dir, Element Creater)
    {
        this.matteryEnergy = matteryEnergy;
        this.mattery = mattery;
        this.dir = dir;
        this.mattery.Creater = Creater;
    }

    public static float GetCircleStrong(TypeMagicCircle type)
    {
        return type switch
        {
            TypeMagicCircle.MIE => 0.413496671566344f,
            TypeMagicCircle.MIH => 0.636619772367581f,
            TypeMagicCircle.MIP => 0.756826728640657f,
            TypeMagicCircle.MIG => 0.826993343132688f,
            TypeMagicCircle.MIU => 1f,
            _ => float.NaN,
        };
    }



}








public enum TypeMagicCircle
{
    MIE,
    MIH,
    MIP,
    MIG,
    MIU
}
[Serializable]


public class ExceptionReadSpeel
{
    public string codeError;

    public ExceptionReadSpeel(string codeError)
    {
        this.codeError = codeError;
    }
}
public class FormuLexer
{
    public SpellActivator spellActivator;
    public class Token
    {
        public string type;
        public object value;
        public ModifyParam modify = ModifyParam.Const;
        public Token(string type, object value)
        {
            this.type = type;
            this.value = value;

        }
    }
    public string formule;
    public int pos;
    public string Localsum;
    public string typesum;
    public List<Token> element = new List<Token>();
    public PPR Localppr;
    public Interpreter interpreter;
    public FormuLexer(string formule, SpellActivator spellActivator)
    {
        this.formule = formule;
        this.spellActivator = spellActivator;
        interpreter = new();
    }
    public void Tonazide()
    {
        do
        {
            if (formule.Length <= 0) return;
            char current = formule[pos];
            switch (current)
            {
                case '+':
                    AddToken(new Token("operator", current));
                    break;
                case '-':
                    AddToken(new Token("operator", current));
                    break;
                case '/':
                    AddToken(new Token("operator", current));
                    break;
                case '*':
                    AddToken(new Token("operator", current));
                    break;
                case ',':
                    AddToken(new Token("comme", current));
                    break;
                case ';':
                    AddToken(new Token("semicolon", current));
                    break;
                case '.':
                    AddToken(new Token("point", current));
                    break;
                case '(':
                    AddToken(new Token("lp", current));
                    break;
                case ')':
                    AddToken(new Token("rp", current));
                    break;
                case '[':
                    AddToken(new Token("lb", current));
                    break;
                case ']':
                    AddToken(new Token("rb", current));
                    break;
                default:
                    if (!char.IsWhiteSpace(current))
                    {
                        if (char.IsDigit(current))
                        {
                            AddSum(current, "digit");
                        }
                        if (char.IsLetter(current))
                        {
                            AddSum(current, "letter");
                        }
                    }
                    break;
            }
        }
        while (Advance());
        ReturnSum();
    }
    public void AddToken(Token token)
    {
        ReturnSum();
        element.Add(token);
    }
    public void ReturnSum()
    {
        if (Localsum != "")
        {
            element.Add(new Token(typesum, Localsum));
            Localsum = "";
            typesum = "";
        }

    }
    public void AddSum(char s, string type)
    {
        bool nal = typesum == "",
             niew = type != typesum;
        if (nal || niew)
        {
            if (nal & niew)
            {
                typesum = type;
                Localsum += s;
            }
            if (!nal & niew)
            {
                ReturnSum();
                typesum = type;
                Localsum += s;

            }
        }
        else
        {
            Localsum += s;
        }

    }
    public object Parse()
    {
        if (element.Count == 0)
        {
            Tonazide();
        }
        Localppr = NumberField(element);
        return Numberate(Localppr.result.ToString());
    }
    public object ToTextParse()
    {
        if (element.Count == 0)
        {
            Tonazide();
        }
        Localppr = NumberField(element);
        if (!Localppr.texter)
        {
            return Numberate("" + Localppr.result);
        }
        else
        {
            if (Localppr.type.Name == "Vector2")
            {
                return Numberate("" + Localppr.result);
            }
            else
                return Localppr.result;
        }
    }
    public Token NextToken(int i, int move, List<Token> tokens)
    {
        if (i + move < tokens.Count & i + move >= 0)
        {
            return tokens[i + move];
        }
        else return null;
    }
    public List<Param> ListParsePather(InfoCar car)
    {
        List<Param> result = new();
        for (; car.i.i < car.tokens.Count; car.i.i++)
        {

            switch (car.tokens[car.i.i].type)
            {
                case "digit":
                    car.sum += car.tokens[car.i.i].value.ToString();
                    break;
                case "point":
                    car.sum += car.tokens[car.i.i].value.ToString();
                    break;
                case "operator":
                    car.sum += car.tokens[car.i.i].value.ToString();
                    break;
                case "comme":
                    result.Add(new(car.sum, result.Count + ""));
                    car.sum = "";
                    break;
                case "semicolon":
                    result.Add(new(car.sum, result.Count + ""));
                    car.sum = "";
                    break;

                case "lp":
                    if (true)
                    {
                        Token old = NextToken(car.i.i, -1, car.tokens);
                        if (old != null)
                        {
                            if (old.type == "letter")
                            {
                                string namefunc = old.value.ToString();
                                string s = car.tokens[car.i.i].value.ToString();

                                car.i.i++;
                                ModifyParam modify = ModifyParam.Const;
                                List<Param> parames = ListParsePather(new(car.tokens, car.i, car.ppr, s, car.spellActivator, interpreter));
                                InfoCar info = new(namefunc, parames, out modify, spellActivator);
                                info.ppr = car.ppr;
                                car.sum += Magic.FuncBlock(info, interpreter).ToString();
                                modify = info.modify;
                                if (modify == ModifyParam.Dynamic)
                                {
                                    car.ppr.modify = modify;
                                    car.ppr.impotantsaves.Add(new SavePoint(car.sum, car.i.i, car.tokens[car.i.i], modify));
                                }
                            }
                            else
                            {
                                string s = car.tokens[car.i.i].value.ToString();

                                car.i.i++;
                                car.ppr = ParsePather(new(car.tokens, car.i, car.ppr, s, car.spellActivator, interpreter));
                                car.sum += car.ppr.result;
                                /*
                                interpreter.SetDefaultNumberType(DefaultNumberType.Single);
                                object resul = interpreter.Eval(PointFormat.UnParse(car.ppr.result));

                                string name=resul.GetType().Name;
                              
                                switch (resul.GetType().Name)
                                {
                                    case "Single":
                                        car.sum += resul.ToString();
                                        break;
                                    case "Double":
                                        car.sum += resul.ToString();
                                        break;
                                    case "Int32":
                                        car.sum += resul.ToString();
                                        break;
                                    case "Int64":
                                        car.sum += resul.ToString();
                                        break;
                                    case "Vector2":
                                        if (true)
                                        {
                                            Vector2 vectorloc = (Vector2)resul;
                                            string numlet = NumberToLetter(vectorloc.x + vectorloc.y + "");
                                            interpreter.SetVariable(numlet, vectorloc, vectorloc.GetType());
                                            car.ppr.type = vectorloc.GetType();
                                            car.ppr.varible = new(vectorloc.GetType(), numlet, vectorloc);
                                            car.sum += car.ppr.varible.name;
                                        }
                                        break;
                                }
                                */
                            }
                        }
                        else
                        {
                            string s = car.tokens[car.i.i].value.ToString();

                            car.i.i++;
                            car.ppr = ParsePather(new(car.tokens, car.i, car.ppr, s, car.spellActivator, interpreter));
                            car.sum += car.ppr.result;
                            /*
                            interpreter.SetDefaultNumberType(DefaultNumberType.Single);
                            object resul = interpreter.Eval(PointFormat.UnParse(car.ppr.result));

                            switch (resul.GetType().Name)
                            {
                                case "Single":
                                    car.sum += resul.ToString();
                                    break;
                                case "Double":
                                    car.sum += resul.ToString();
                                    break;
                                case "Int32":
                                    car.sum += resul.ToString();
                                    break;
                                case "Int64":
                                    car.sum += resul.ToString();
                                    break;
                                case "Vector2":
                                    if (true)
                                    {
                                        Vector2 vectorloc = (Vector2)resul;
                                        string numlet = NumberToLetter(vectorloc.x + vectorloc.y + "");
                                        interpreter.SetVariable(numlet, vectorloc, vectorloc.GetType());
                                        car.ppr.type = vectorloc.GetType();
                                        car.ppr.varible = new(vectorloc.GetType(), numlet, vectorloc);
                                        car.sum += car.ppr.varible.name;
                                    }
                                    break;
                            }
                            */
                        }
                        break;
                    }
                case "rp":
                    car.sum += car.tokens[car.i.i].value.ToString();
                    Param p = new(car.sum, result.Count + "", car.ppr.modify, car.sum);
                    p.ppr = car.ppr;
                    //i++;
                    result.Add(p);
                    car.sum = "";

                    return result;
                case "lb":
                    if (true)
                    {
                        string s = car.tokens[car.i.i].value.ToString();
                        car.i.i++;

                        string block = ParsePather(new(car.tokens, car.i, car.ppr, s, car.spellActivator, interpreter)).result.ToString();

                        ModifyParam modifyParam;
                        Param param = null;
                        InfoCar info = new(block, param, out modifyParam, spellActivator);
                        info.ppr = car.ppr;
                        car.sum += Magic.Block(info, car.interpreter).ToString();
                        modifyParam = info.modify;
                        if (modifyParam == ModifyParam.Dynamic)
                        {
                            car.ppr.modify = modifyParam;
                            car.ppr.impotantsaves.Add(new SavePoint(car.sum, car.i.i, car.tokens[car.i.i], modifyParam));
                        }

                    }
                    break;
                case "rb":

                    car.sum += car.tokens[car.i.i].value.ToString();

                    p = new(car.sum, result.Count + "", car.ppr.modify, car.sum);
                    p.ppr = car.ppr;
                    //i++;
                    result.Add(p);
                    car.sum = "";
                    return result;
                case "letter":
                    //sum += tokens[i].value.ToString();
                    Token next = NextToken(car.i.i, 1, car.tokens);
                    Token old1 = NextToken(car.i.i, -1, car.tokens);
                    if (next != null)
                    {
                        switch (next.type)
                        {
                            case "operator":
                                Param par = spellActivator.GetParameter(car.tokens[car.i.i].value.ToString());
                                if (par != null)
                                   
                                    {
                                    if (par.value != null)
                                        switch (par.value.GetType().Name)
                                        {
                                            case "Vector2":
                                                Vector2 v = (Vector2)par.value;
                                                car.sum += par.ppr.varible.name;
                                                car.ppr.varible = par.ppr.varible;
                                                interpreter.SetVariable(par.ppr.varible.name, par.ppr.varible.value, v.GetType());
                                                car.ppr.type = v.GetType();
                                                break;
                                            case "Single":
                                                car.sum += par.value + "";
                                                break;

                                        }

                                        if (par.modify == ModifyParam.Dynamic)
                                        {
                                            car.ppr.modify = par.modify;

                                        }
                                    }
                                    else
                                    {
                                        car.sum += car.tokens[car.i.i].value.ToString();
                                        if (old1 != null)
                                        {
                                            if (old1.type != "lb")
                                            {


                                                car.ppr.texter = true;
                                            }
                                        }
                                        else
                                        {
                                            car.ppr.texter = true;
                                        }


                                    }
                                break;
                            case "lp":
                                continue;
                                break;
                            default:
                                par = spellActivator.GetParameter(car.tokens[car.i.i].value.ToString());
                                if (par != null)
                                  
                                    {
                                    if (par.value != null)
                                        switch (par.value.GetType().Name)
                                        {
                                            case "Vector2":
                                                Vector2 v = (Vector2)par.value;
                                                car.sum += par.ppr.varible.name;
                                                interpreter.SetVariable(par.ppr.varible.name, par.ppr.varible.value, v.GetType());
                                                car.ppr.type = v.GetType();
                                                car.ppr.varible = par.ppr.varible;
                                                break;
                                            case "Single":
                                                car.sum += par.value + "";
                                                break;

                                        }

                                        if (par.modify == ModifyParam.Dynamic)
                                            car.ppr.modify = par.modify;
                                    }
                                    else
                                    {
                                        car.sum += car.tokens[car.i.i].value.ToString();
                                        if (old1 != null)
                                        {
                                            if (old1.type != "lb")
                                            {


                                                car.ppr.texter = true;
                                            }
                                        }
                                        else
                                        {
                                            car.ppr.texter = true;
                                        }



                                    }

                                break;

                        }
                    }
                    else
                    {
                        Param par = spellActivator.GetParameter(car.tokens[car.i.i].value.ToString());
                        if (par != null)
                            
                            {
                            if (par.value != null)
                                switch (par.value.GetType().Name)
                                {
                                    case "Vector2":
                                        Vector2 v = (Vector2)par.value;
                                        car.sum += par.ppr.varible.name;
                                        interpreter.SetVariable(par.ppr.varible.name, par.ppr.varible.value, v.GetType());
                                        car.ppr.type = v.GetType();
                                        car.ppr.varible = par.ppr.varible;
                                        break;
                                    case "Single":
                                        car.sum += par.value + "";
                                        break;

                                }

                            }
                            else
                            {
                                car.sum += car.tokens[car.i.i].value.ToString();
                                if (old1 != null)
                                {
                                    if (old1.type != "lb")
                                    {


                                        car.ppr.texter = true;
                                    }
                                }
                                else
                                {
                                    car.ppr.texter = true;
                                }


                            }
                        break;
                    }
                    break;

            }
            car.ppr.saves.Add(new SavePoint(car.sum, car.i.i));
        }
        car.ppr.saves.Add(new SavePoint(car.sum, car.i.i));
        result.Add(new(car.sum, result.Count + ""));

        car.sum = "";
        return result;
    }

    //  public PPR ParsePather(List<Token> tokens, ref int i, PPR ppr, string sum = "")
    public static string NumberToLetter(string numer)
    {
        string sum = "";
        for (int i = 0; i < numer.Length; i++)
        {
            if (char.IsDigit(numer[i]))
            {
                sum += (char)('A' + int.Parse(numer[i] + ""));
            }
            else
            {
                if (char.IsLetter(numer[i]))
                {
                    sum += numer[i];
                }
                else
                {
                    sum += (char)('A' + 10);
                }
            }
        }
        return sum;
    }
    public PPR ParsePather(InfoCar car)
    {

        for (; car.i.i < car.tokens.Count; car.i.i++)
        {

            switch (car.tokens[car.i.i].type)
            {
                case "digit":
                    car.sum += car.tokens[car.i.i].value.ToString();
                    break;
                case "operator":
                    car.sum += car.tokens[car.i.i].value.ToString();
                    break;
                case "point":
                    car.sum += car.tokens[car.i.i].value.ToString();
                    break;
                case "comme":
                    car.sum += car.tokens[car.i.i].value.ToString();
                    break;
                case "semicolon":

                    car.ppr.type = typeof(Vector2);
                    string s1 = car.tokens[car.i.i].value.ToString();
                    ModifyParam modifyc = ModifyParam.Const;
                    car.i.i++;

                    List<Param> list = ListParsePather(new(car.tokens, car.i, car.ppr, s1, car.spellActivator, interpreter));
                    if (list[0].modify == ModifyParam.Dynamic) modifyc = ModifyParam.Dynamic;
                    if (list[1].modify == ModifyParam.Dynamic) modifyc = ModifyParam.Dynamic;
                    Vector2 vector = new(Convert.ToSingle(list[0].value), Convert.ToSingle(list[1].value));
                    string res = NumberToLetter(vector.x + vector.y + "");
                    interpreter.SetVariable(res, vector, vector.GetType());
                    car.sum += res;
                    //у меня нет возможности выслеживания вектора
                    break;
                case "lp":
                    if (true)
                    {
                        Token old = NextToken(car.i.i, -1, car.tokens);
                        if (old != null)
                        {
                            if (old.type == "letter")
                            {
                                string namefunc = old.value.ToString();
                                string s = car.tokens[car.i.i].value.ToString();

                                car.i.i++;
                                ModifyParam modify = ModifyParam.Const;
                                List<Param> parames = ListParsePather(new(car.tokens, car.i, car.ppr, s, car.spellActivator, interpreter));
                                InfoCar info = new(namefunc, parames, out modify, spellActivator);
                                info.ppr = car.ppr;
                                car.sum += Magic.FuncBlock(info, interpreter).ToString();
                                modify = info.modify;
                                if (modify == ModifyParam.Dynamic)
                                {
                                    car.ppr.modify = modify;
                                    car.ppr.impotantsaves.Add(new SavePoint(car.sum, car.i.i, car.tokens[car.i.i], modify));
                                }
                            }
                            else
                            {
                                string s = car.tokens[car.i.i].value.ToString();

                                car.i.i++;
                                car.ppr = ParsePather(new(car.tokens, car.i, car.ppr, s, car.spellActivator, interpreter));
                                car.sum += car.ppr.result;
                                /*
                                interpreter.SetDefaultNumberType(DefaultNumberType.Single);
                                object result = interpreter.Eval(PointFormat.UnParse(car.ppr.result));
                                switch (result.GetType().Name)
                                {
                                    case "Single":
                                        car.sum += result.ToString();
                                        break;
                                    case "Double":
                                        car.sum += result.ToString();
                                        break;
                                    case "Int32":
                                        car.sum += result.ToString();
                                        break;
                                    case "Int64":
                                        car.sum += result.ToString();
                                        break;
                                    case "Vector2":
                                        if (true)
                                        {
                                            Vector2 vectorloc = (Vector2)result;
                                            string numlet = NumberToLetter(vectorloc.x + vectorloc.y + "");
                                            interpreter.SetVariable(numlet, vectorloc, vectorloc.GetType());
                                            car.ppr.type = vectorloc.GetType();
                                            car.ppr.varible = new(vectorloc.GetType(), numlet, vectorloc);
                                            car.sum += car.ppr.varible.name;
                                        }
                                        break;
                                }
                                */
                            }
                        }
                        else
                        {
                            string s = car.tokens[car.i.i].value.ToString();

                            car.i.i++;
                            car.ppr = ParsePather(new(car.tokens, car.i, car.ppr, s, car.spellActivator, interpreter));
                            car.sum += car.ppr.result;
                            /*
                            object result = interpreter.Eval(PointFormat.UnParse( car.ppr.result));
                            switch (result.GetType().Name)
                            {
                                case "Single":
                                    car.sum += result.ToString();
                                    break;
                                case "Double":
                                    car.sum += result.ToString();
                                    break;
                                case "Int32":
                                    car.sum += result.ToString();
                                    break;
                                case "Int64":
                                    car.sum += result.ToString();
                                    break;
                                case "Vector2":
                                    if (true)
                                    {
                                        Vector2 vectorloc = (Vector2)result;
                                        string numlet = NumberToLetter(vectorloc.x + vectorloc.y + "");
                                        interpreter.SetVariable(numlet, vectorloc, vectorloc.GetType());
                                        car.ppr.type = vectorloc.GetType();
                                        car.ppr.varible = new(vectorloc.GetType(), numlet, vectorloc);
                                        car.sum += car.ppr.varible.name;
                                    }
                                    break;
                            }
                            */
                        }
                        break;
                    }
                case "rp":

                    car.sum += car.tokens[car.i.i].value.ToString();
                    //i++;

                    car.ppr.result = car.sum.ToString();
                    return car.ppr;
                case "lb":
                    if (true)
                    {
                        string s = car.tokens[car.i.i].value.ToString();
                        car.i.i++;

                        string block = ParsePather(new(car.tokens, car.i, car.ppr, s, car.spellActivator, interpreter)).result.ToString();

                        ModifyParam modifyParam;
                        Param param = null;
                        InfoCar info = new(block, param, out modifyParam, spellActivator);
                        info.ppr = car.ppr;
                        block = Magic.Block(info, car.interpreter).ToString();

                        car.sum += block;
                        modifyParam = info.modify;

                        if (modifyParam == ModifyParam.Dynamic)
                        {
                            car.ppr.modify = modifyParam;
                            car.ppr.impotantsaves.Add(new SavePoint(car.sum, car.i.i, car.tokens[car.i.i], modifyParam));
                        }

                    }
                    break;
                case "rb":

                    car.sum += car.tokens[car.i.i].value.ToString();
                    //i++;

                    car.ppr.result = car.sum;
                    return car.ppr;
                case "letter":
                    //sum += tokens[i].value.ToString();
                    Token next = NextToken(car.i.i, 1, car.tokens);
                    Token old1 = NextToken(car.i.i, -1, car.tokens);
                    if (next != null)
                    {
                        switch (next.type)
                        {
                            case "operator":
                                Param par = spellActivator.GetParameter(car.tokens[car.i.i].value.ToString());
                                if (par != null)
                                   
                                    {
                                    if (par.value != null)
                                        switch (par.value.GetType().Name)
                                        {
                                            case "Vector2":
                                                Vector2 v = (Vector2)par.value;
                                                car.sum += par.ppr.varible.name;
                                                interpreter.SetVariable(par.ppr.varible.name, par.ppr.varible.value, v.GetType());
                                                car.ppr.type = v.GetType();
                                                car.ppr.varible = par.ppr.varible;
                                                break;
                                            case "Single":
                                                car.sum += par.value + "";
                                                break;

                                        }

                                        if (par.modify == ModifyParam.Dynamic)
                                        {
                                            car.ppr.modify = par.modify;

                                        }
                                    }
                                    else
                                    {
                                        car.sum += car.tokens[car.i.i].value.ToString();
                                        if (old1 != null)
                                        {
                                            if (old1.type != "lb")
                                            {


                                                car.ppr.texter = true;
                                            }
                                        }
                                        else
                                        {
                                            car.ppr.texter = true;
                                        }


                                    }
                                break;
                            case "lp":
                                continue;
                                break;
                            default:
                                par = spellActivator.GetParameter(car.tokens[car.i.i].value.ToString());
                                if (par != null)
                                   
                                    {
                                    if (par.value != null)
                                        switch (par.value.GetType().Name)
                                        {
                                            case "Vector2":
                                                Vector2 v = (Vector2)par.value;
                                                car.sum += par.ppr.varible.name;
                                                interpreter.SetVariable(par.ppr.varible.name, par.ppr.varible.value, v.GetType());
                                                car.ppr.type = v.GetType();
                                                car.ppr.varible = par.ppr.varible;
                                                break;
                                            case "Single":
                                                car.sum += par.value + "";
                                                break;

                                        }

                                        if (par.modify == ModifyParam.Dynamic)
                                            car.ppr.modify = par.modify;
                                    }
                                    else
                                    {
                                        car.sum += car.tokens[car.i.i].value.ToString();
                                        if (old1 != null)
                                        {
                                            if (old1.type != "lb")
                                            {


                                                car.ppr.texter = true;
                                            }
                                        }
                                        else
                                        {
                                            car.ppr.texter = true;
                                        }



                                    }

                                break;

                        }
                    }
                    else
                    {
                        Param par = spellActivator.GetParameter(car.tokens[car.i.i].value.ToString());
                        if (par != null)

                        {
                            if (par.value != null)
                                switch (par.value.GetType().Name)
                                {
                                    case "Vector2":
                                        Vector2 v = (Vector2)par.value;
                                        car.sum += par.ppr.varible.name;
                                        interpreter.SetVariable(par.ppr.varible.name, par.ppr.varible.value, v.GetType());
                                        car.ppr.type = v.GetType();
                                        car.ppr.varible = par.ppr.varible;
                                        break;
                                    case "Single":
                                        car.sum += par.value + "";
                                        break;

                                }

                        }
                        else
                        {
                            car.sum += car.tokens[car.i.i].value.ToString();
                            if (old1 != null)
                            {
                                if (old1.type != "lb")
                                {


                                    car.ppr.texter = true;
                                }
                            }
                            else
                            {
                                car.ppr.texter = true;
                            }



                        }
                        break;
                    }
                    break;

            }
            car.ppr.saves.Add(new SavePoint(car.sum, car.i.i));
        }
        car.ppr.saves.Add(new SavePoint(car.sum, car.i.i));
        car.ppr.result = car.sum;
        return car.ppr;
    }
    public PPR TexterNumberField(List<Token> tokens)
    {

        tokens = new List<Token>(tokens.ToArray());
        Step i = new();
        i.i = 0;
        if (Localppr == null)
        {
            return ParsePather(new(tokens, i, new PPR(), "", spellActivator, interpreter));
        }
        else
        {
            int r = Localppr.saves[Localppr.saves.Count - 1].current;
            return ParsePather(new(tokens, i, new PPR(), Localppr.saves[Localppr.saves.Count - 1].sum, spellActivator, interpreter));
        }






    }
    public PPR NumberField(List<Token> tokens)
    {

        tokens = new List<Token>(tokens.ToArray());
        Step i = new();
        i.i = 0;
        if (Localppr == null)
        {
            return ParsePather(new(tokens, i, new PPR(), "", spellActivator, interpreter));
        }
        else
        {


            return ParsePather(new(tokens, i, Localppr, "", spellActivator, interpreter));

        }






    }
    public bool Advance()
    {
        pos++;
        bool res = pos < formule.Length;

        return res;
    }

    public object Numberate(string s)
    {



        string f = PointFormat.UnParse(s);




        interpreter.SetDefaultNumberType(DefaultNumberType.Single);
        object result = null;
        try
        {
            result = interpreter.Eval(f);
        }
        catch (Exception ex)
        {

        }
        if (result != null)
        {
            if (result.GetType().Name == "Vector2")
            {
                Vector2 v = (Vector2)result;

                Localppr.varible = new(result.GetType(), NumberToLetter(v.x + v.y + ""), result);
            }
            return result;
        }
        else return null;
    }
}
public class SavePoint
{
    public string sum;
    public int current;
    public ModifyParam modify;
    public FormuLexer.Token token;
    public SavePoint(string sum, int current)
    {
        this.sum = sum;
        this.current = current;
    }
    public SavePoint(string sum, int current, FormuLexer.Token token, ModifyParam modify)
    {
        this.sum = sum;
        this.current = current;
        this.token = token;
        this.modify = modify;
    }
}
public class PPR
{
    public List<SavePoint> saves = new List<SavePoint>(), impotantsaves = new();
    public string result;
    public bool texter;
    public ModifyParam modify;
    public Type type = typeof(float);
    public Varible varible;

}

public enum TypeMattery
{
    Energy,
    BodyObject
}
