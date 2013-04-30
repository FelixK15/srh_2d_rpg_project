using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RpgGame.Dialogs
{
    class Dialog
    {
        public List<DialogNode> DialogNodes;
        public int currentDialogNodeIndex;
        public int dialogID;

        public Dialog()
        {
            DialogNodes = new List<DialogNode>();
            currentDialogNodeIndex = -1;
        }

        public Dialog(int ID, List<DialogNode> dialogNodeList)
        {
            dialogID = ID;
            DialogNodes = dialogNodeList;
        }

        public void addDialogNode(DialogNode newNode)
        {
            DialogNodes.Add(newNode);
        }

        public void setCurrentDialogNode(int ID)
        {
            currentDialogNodeIndex = ID;
        }

        public bool NextDialogNode()
        {
            if (currentDialogNodeIndex < DialogNodes.Count)
            {
                currentDialogNodeIndex++;
                return true;
            }
            else return false;    
        }

        public void StartDialog()
        {
            DialogBox.startDialog(this);
        }
    }
}
