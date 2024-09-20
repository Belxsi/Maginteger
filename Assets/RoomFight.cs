using System.Collections.Generic;
using UnityEngine;
public class RoomFight : MonoBehaviour
{

    public List<NPC> npcs = new();
    public AutoWall walls;
    public StateRoomFight state = StateRoomFight.Wait;
    public Room room;
    public Collider2D Triggercollider;
    public bool wallsHide;
    void Start()
    {
        AllSetActiveNPC(false);
     
   
        walls.chunck = room.chunck;
        walls.SetActiveWall(false);
    }
    public void AllSetActiveNPC(bool trfl)
    {
        for(int i = 0; i < npcs.Count; i++)
        {
            npcs[i].gameObject.SetActive(trfl);
           // npcs[i].behaviour.GetWallOffsets();
        }
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (state == StateRoomFight.Wait)
        {
            if (collision.CompareTag("Player"))
            {
                state = StateRoomFight.Process;
                AllSetActiveNPC(true);
                Triggercollider.enabled = false;
                room.SetInvisSpecial(-Time.deltaTime,false);
                walls.SetActiveWall(true);
            }
        }
    }
    public void WakeUp(List<NPC> npcs)
    {
        this.npcs = npcs;
        state = StateRoomFight.Wait;
        enabled = true;
    }
    void Update()
    {
        if(!wallsHide)
        if (GeneratorLevel.navmeshgen)
        {
                wallsHide = true;
                walls.SetActiveWall(false);
        }
        switch (state)
        {
            case StateRoomFight.Wait:
                Triggercollider.enabled = true;
                break;
            case StateRoomFight.Process:

                if (npcs.Count <= 0)
                {
                    if(Player.TryGetPlayer())
                    CameraFollow.Follower = Player.me.gameObject;
                    
                    StartCoroutine( CameraFollow.SetSmoothSize(0.1f,9f));
                    room.SetInvisSpecial(Time.deltaTime,true);
                    state = StateRoomFight.Sleep;
                    walls.SetActiveWall(false);
                    enabled = false;

                }
                else
                {
                    CameraFollow.Follower = gameObject;
                    StartCoroutine(CameraFollow.SetSmoothSize(0.1f, 15f));
                }
                break;
        }
    }
    public enum StateRoomFight
    {
        Wait,
        Process,
        Sleep
    }
}
