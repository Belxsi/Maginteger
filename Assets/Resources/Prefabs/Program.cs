using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using UnityEngine;
using System.Collections;

namespace RandomSpell
{
    class Program:MonoBehaviour
    {
        public string path ;

       public  MainRS mainRS = new MainRS("MIE O ");
        public List<string> spells = new List<string>();
        public string spell = "";
        public bool active;
        public void Awake()
        {
             path = Application.streamingAssetsPath + "\\" + "spells" + new System.Random().Next(int.MinValue, int.MaxValue) + ".txt";
            File.Create(path);
        }
        public void Update()
        {
            StartCoroutine(UpdateGenerate());
        }
        public int i = 0;
        public IEnumerator UpdateGenerate()
        {
            if (!active)
            {

                active = true;

                







                Player player = Player.me;
                player.parameters = new Parameters(1000, 12, 5, 30);
                player.Awake();

               // GenerateSpell(mainRS, out spell);


                if (i < GeneratorScroll.me.spells.Count)
                {
                    spell = GeneratorScroll.me.spells[i];
                    i++;
                }
                


                TestingSpell test = new TestingSpell(spell);
                IEnumeratorObject ieo = new();
                yield return StartCoroutine(test.IsGood(ieo));
                if (ieo.Get<bool>())
                {

                    test = new TestingSpell(spell);
                    ieo = new();
                    yield return StartCoroutine(test.IsGood(ieo));
                    if (ieo.Get<bool>())
                    {
                        spells.Add(spell);

                        File.WriteAllText(path, "");
                        foreach (var item in spells)
                        {


                            File.AppendAllText(path, item + '\n');

                        }
                    }

             
                }




                
                active = false;
            }
            

            
        }
        public static void GenerateSpell(MainRS mainRS,out string spell)
        {
            spell = "";
            while (true)
            {
                string buf = mainRS.GenerateRandomSpell();
                



                    spell = buf;

                break;

            }
        }
        
        public class TestingSpell
        {
            public Energy energy;
            public string spell;
            public float start_time;
            public bool AvaybilityFloat(float value)
            {
                if (float.IsNaN(value)) return false;
                if (float.IsInfinity(value)) return false;
                return true;
            }
            public TestingSpell(string spell)
            {
                this.spell = spell;
                start_time = Time.time;
            }
            public float GetTime()
            {
                return Time.time - start_time;
            }
            public bool NoErrorCast()
            {
                try
                {
                    Player.me.cm.FomritMagic(spell, Player.me, Player.me.cm);
                    energy = Player.me.cm.mattery.energy;

                }
                catch(Exception ex)
                {
                    return false;
                }
                return true;
            }
            public bool NoErrorFire()
            {
                try
                {
                    Player.me.cm.Fire(Player.me.cm);

                }
                catch (Exception ex)
                {
                    return false;
                }
                return true;
            }
            public IEnumerator IsGood(IEnumeratorObject ieo)
            {


               if(! NoErrorCast())
                {
                    ieo.Set(false);
                    yield break;
                }
                  
                    yield return new WaitForSeconds(0.1f);
                if (!NoErrorFire())
                {
                    ieo.Set(false);
                    yield break;
                }


                float maxspeed = 0, maximpuls = 0 ;
                while (true)
                {
                    if (ieo.stop) break;
                    yield return new WaitForSeconds(Time.deltaTime);
                    try
                    {
                        if(energy.magic.spellActivator.Use()!=null) ieo.Set(false); 
                        // Console.WriteLine("------------");
                        if (!AvaybilityFloat(energy.transform.position.x)) ieo.Set(false);
                        if (!AvaybilityFloat(energy.transform.position.y)) ieo.Set(false);
                        if (!AvaybilityFloat(energy.transform.localScale.x)) ieo.Set(false);
                        if (!AvaybilityFloat(energy.transform.localScale.y)) ieo.Set(false);
                        if (!AvaybilityFloat(energy.Phys.rg.velocity.x)) ieo.Set(false);
                        if (!AvaybilityFloat(energy.Phys.rg.velocity.y)) ieo.Set(false);
                        float speed = energy.Phys.rg.velocity.magnitude,
                            impuls= energy.magic.spellActivator.GetImpulsMany();
                        if (!AvaybilityFloat(impuls)) ieo.Set(false);
                        if (speed > maxspeed) maxspeed = speed;
                        if (impuls > maximpuls) maximpuls = impuls;
                        float valE = 0, maxE = 0, gcs = 0, gcp = 0;
                        valE = Player.me.life.valueEnergy;
                        maxE = Player.me.life.parameters.energy;
                        if (energy.magic.spellActivator.TryGetParameter("MI", out Param mi))
                        {
                            gcs = Magic.GetCircleStrong((TypeMagicCircle)mi.value);
                            gcp = Magic.GetCircleSpeed((TypeMagicCircle)mi.value);
                        }
                        if (valE / maxE < (1 - gcs)/2) ieo.Set(false);
                        if (!AvaybilityFloat(valE)) ieo.Set(false);
                        if (!AvaybilityFloat(maxE)) ieo.Set(false);
                        if (Player.me.GetDolyLife()!=1)  ieo.Set(false);
                        // Console.WriteLine("pos: " + energy.transform.position);
                        // Console.WriteLine("scale: " + energy.transform.localScale);
                        // Console.WriteLine("velocity: " + energy.Phys.velocity);
                        if (GetTime() > 3F)
                        {
                            if (maxspeed > 0.1f&maximpuls>0.5f)
                            {
                                ieo.Set(true);
                            }else
                            ieo.Set(false);
                        }
                        if (energy == null)
                            if (GetTime() < 0.5F)
                            {
                                ieo.Set(false);
                            }
                            else
                            {
                                if (maxspeed > 0.1f & maximpuls > 0.5f)
                                    ieo.Set(true);
                            }
                        if(energy.UpdateSpeel!=null)
                        if (energy.UpdateSpeel.trfl == false)
                        {
                            if (GetTime() < 0.5F)
                            {
                                ieo.Set(false);
                            }

                        }
                    }catch(Exception ex)
                    {
                        ieo.Set(false);
                    }
                   // Console.WriteLine("+++++++++++++");
                   
                }
                

            }
        }
    }
}
public class IEnumeratorObject{
    public object obj;
    public bool stop;
    public void Set(object set)
    {
        obj = set;
        stop = true;
    }
    public T Get<T>()
    {
        return (T)(obj);
    }
}
