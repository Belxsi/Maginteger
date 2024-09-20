namespace ffff
{
    using System;
    using System.Collections.Generic;
    using Apex.AI;
    using Apex.AI.Components;
    using UnityEngine;

    public class NPCContextProvider : IAIContext
    {
        private IAIContext _context;

        public NPCContextProvider(NPC npc)
        {
            this.npc = npc;
            if (Player.TryGetPlayer())
                player = Player.me;
            if(Player.TryGetPlayer())
            energies = Player.me.energies;
        }
        public NPC npc;
        public List<Energy> energies;
        public Player player;
        public IAIContext GetContext(Guid aiId)
        {
            return _context;
        }
    }
}
