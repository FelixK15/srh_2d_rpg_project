using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RpgGame.Dialogs
{
    class DialogNode
    {
        public string Actor{ get; set; }
        public string Text{ get; set; }
        public int dialogNodeID;

        public DialogNode(string actor, string text, int id)
        {
            this.Actor = actor;
            this.Text = text;
            dialogNodeID = id;
        }
    }
}
