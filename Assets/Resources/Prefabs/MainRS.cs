using System;
using System.Collections.Generic;
using System.Threading;
namespace RandomSpell
{

    public class MainRS
    {
        public List<OperatorRS> operators;
        public List<FuncRS> functions;
        public List<SpacerRS> spacers;
        public Random random = new Random();
        public void InitBaseElementsRS()
        {
            operators = new List<OperatorRS>()
            {
              new OperatorRS("+") ,
               new OperatorRS("-") ,
                new OperatorRS("*") ,
                new OperatorRS("/")

            };
            functions = new List<FuncRS>()
            {
              new FuncRS("SIN" +
              "")


            };
            spacers = new List<SpacerRS>()
            {
              new SpacerRS("[TIME]","T","T=[TIME]")
            //  new SpacerRS("[TIMEGLOBAL]","G","G=[TIMEGLOBAL]")

            };

        }
        public string OffSpell;
        public MainRS(string OffSpell)
        {
            this.OffSpell = OffSpell;
            InitBaseElementsRS();
        }
        public int max_lenght_exp =4;
        public static List<string> vars = new List<string>(), formuls = new List<string>();
        public string AddForm(string value, int index)
        {
            string result = "";

            result = Recurse(new InfoExpress(index, value, "")).result;



            return "FORM(" + value + "=" + result + ")";
        }
        public static string AddForm(string seter)
        {
            string result = "";

            result = seter;



            return "FORM(" + result + ")";
        }
        public string RandomSpacer()
        {


            SpacerRS rs = spacers[random.Next(0, spacers.Count)];
            if (rs.form != "")
            {
                if (!vars.Contains(rs.vr))
                {


                    rs.SetAutoVarible();
                    return rs.vr;
                }
                else
                {
                    return rs.vr;
                }


            }
            else
                return rs.GetValue();
        }
        public string RandomFunc()
        {
            return functions[random.Next(0, functions.Count)].GetValue(Recurse(new InfoExpress()).result);
        }
        public string GetNameVarible()
        {
            string result = "";
            do
            {
                result = "" + (char)random.Next('A', 'Z');
            }
            while (result == "R" || result == "E" || result == "V" || result == "T" || result == "G");
            return result;
        }
        public float GetValidateNumber(int l)
        {

            float result = 0;
            do
            {
                result = (float)random.Next(-25 * l, 25);
            } while (result == 0);
            return result;
        }
        public class InfoExpress
        {
            public float index;
            public string exep;

            public string result;
            public bool good = false;
            public InfoExpress(float index, string exep, string result)
            {
                this.index = index;
                this.exep = exep;

                this.result = result;
            }
            public InfoExpress()
            {

            }

        }
        public object locker = new object();
        public Mutex re = new Mutex();

        public void RandomExpress(object obj)
        {
            
            InfoExpress f = (InfoExpress)obj;

            string result = "";
            int cycles = random.Next(1, max_lenght_exp + 1);
            for (int i = 0; i < cycles; i++)
            {


                f.result = "";

                string value = "";
                bool repeat = true;
                while (repeat)
                {
                    switch ((TypeERS)(random.Next(0, 4)))
                    {
                        case TypeERS.Number:
                            int l = 1;
                            if (f.result.Length > 0)
                                if (f.result[f.result.Length - 1] == '-') l = -1;
                            value = new NumberRS(GetValidateNumber(l)).GetValue() + "";
                            if (value[0] == '-') value = "(" + value + ")";
                            repeat = false;
                            break;
                        case TypeERS.Varible:
                            if (random.NextDouble() > 0.5f)
                            {
                                value = new VaribleRS(GetNameVarible()).GetValue() + "";

                                string s = value;
                                if (!vars.Contains(s))
                                {
                                    vars.Add(s);
                                    formuls.Add(AddForm(s, vars.Count - 1));
                                    repeat = false;
                                }
                                break;
                            }
                            else
                            {
                                try
                                {
                                    if (vars.Count > 1)
                                    {
                                        string buf = "";
                                        int ind = 0;
                                        do
                                        {
                                            ind = random.Next(0, vars.Count);
                                            buf = vars[ind];
                                            if (ind < f.index)
                                            {

                                            }
                                            else buf = "Stop";
                                        } while (buf == f.exep);
                                        if (buf != "Stop")
                                        {
                                            value = buf;
                                            repeat = false;
                                        }
                                    }
                                }
                                catch (Exception exp)
                                {

                                }
                            }
                            break;
                        case TypeERS.Func:
                            if (random.NextDouble() > 0.5f)
                            {
                                value = RandomFunc();
                                repeat = false;
                            }
                            break;

                        case TypeERS.Spacer:
                            value = RandomSpacer();
                            repeat = false;
                            break;
                    }
                }
                f.result += value;
                if (i + 1 >= cycles) { }
                else
                {
                    f.result += operators[random.Next(1, operators.Count)].GetValue<string>();
                }
                f.good = true;





                result += f.result;


            }
            f.result = result;
            
        }
        Thread thread1;
        int countRec = 0;
        public InfoExpress Recurse(InfoExpress info)
        {
            RandomExpress(info);
            return info;
        }
        public string BaseGenValue(string offset)
        {
            string result = "";


            result = Recurse(new InfoExpress()).result;




            return offset + "(" + result + ")";
        }
        public enum TypeERS
        {
            Number,
            Varible,
            Func,
            Spacer
        }
        public string Var()
        {
            string result = "";
            for (int i = 0; i < vars.Count; i++)
            {
                result += vars[i];
                if (i + 1 >= vars.Count) break;
                result += ",";


            }
            return "VAR(" + result + ")";
        }
        public string Formule()
        {
            string result = "";
            for (int i = 0; i < formuls.Count; i++)
            {
                result += formuls[i];
                if (i + 1 >= formuls.Count) break;
                result += " ";


            }
            return result;
        }
        public string GenerateRandomSpell()
        {
            thread1 = new Thread(new ParameterizedThreadStart(RandomExpress));
            string result = "";
            vars.Clear();
            formuls.Clear();
            string V = BaseGenValue("V"),
                R = BaseGenValue("R"),
                E = BaseGenValue("E");
            string cut_var = "";
            if (vars.Count > 0) cut_var = Var() + " " + Formule() + " ";
            result += cut_var + V + " " + R + " " + E;



            return OffSpell + result;
        }



    }

    public class VaribleRS : ElementRS
    {
        public VaribleRS(object value)
        {
            this.value = value;
        }
        public string GetValue()
        {
            return IncapsuliteValue<string>();
        }
        public void SetValue(string newvalue)
        {
            value = newvalue;
        }

    }
    public class FuncRS : ElementRS
    {
        public FuncRS(object value)
        {
            this.value = value;
        }
        public string GetValue(string val)
        {
            return IncapsuliteValue<string>() + "(" + val + ")";
        }
        public void SetValue(string newvalue)
        {
            value = newvalue;
        }

    }
    public class SpacerRS : ElementRS
    {
        public string vr = "", form = "";
        public SpacerRS(object value, params object[] list)
        {
            this.value = value;
            if (list.Length > 0)
                vr = list[0].ToString();
            if (list.Length > 1)
                form = list[1].ToString();

        }
        public string GetValue()
        {
            return IncapsuliteValue<string>();
        }
        public void SetAutoVarible()
        {
            MainRS.vars.Add(vr);
            MainRS.formuls.Add(MainRS.AddForm(form));
        }
        public void SetValue(string newvalue)
        {
            value = newvalue;
        }

    }
    public class NumberRS : ElementRS
    {
        public NumberRS(object value)
        {
            this.value = value;
        }
        public float GetValue()
        {
            return IncapsuliteValue<float>();
        }
        public void SetValue(float newvalue)
        {
            value = newvalue;
        }

    }
    public class OperatorRS : ElementRS
    {
        public OperatorRS(object value)
        {
            this.value = value;
        }
        public string GetValue()
        {
            return IncapsuliteValue<string>();
        }
        public void SetValue(string newvalue)
        {
            value = newvalue;
        }

    }
    public abstract class ElementRS
    {
        public object value;

        public T IncapsuliteValue<T>()
        {
            return (T)value;
        }
        public T GetValue<T>()
        {
            return IncapsuliteValue<T>();
        }
        public void SetValue(object newvalue)
        {
            value = newvalue;
        }

    }
}
