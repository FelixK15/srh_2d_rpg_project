using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using RpgGame.GameComponents;
using System.IO.Compression;
using Dialog_Importer.XML_Objects;
using RpgGame.Manager;
using RpgGame.Dialogs;

namespace RpgGame.Tools
{
    class DialogLoader
    {
        public List<Dialog> Dialogs { get; set; }
        private List<DialogNode> DialogNodeList { get; set; }


        public static void Initialize()
        {
            
        }

        public DialogLoader()
        {
            Dialogs = new List<Dialog>();
            DialogNodeList = new List<DialogNode>();
        }

        public DialogLoader(string filename)
            : this()
        {
            LoadXML(filename);
        }

        public void LoadXML(string filename)
        {
            XMLDialogs AllDialogs = RpgGame.ContentManager.Load<XMLDialogs>(filename);

            foreach (XMLDialog dialog in AllDialogs.DialogList)
            {
                _CreateDialogList(dialog);
            }
        }

        private void _CreateDialogList(XMLDialog InputDialog)
        {
            DialogNodeList.Clear();
            List<DialogNode> tmpList = new List<DialogNode>();
            foreach(XMLNode node in InputDialog.DialogNodes){
                _CreateNodeList(node);
            }
            foreach(DialogNode node in DialogNodeList){
                tmpList.Add(node);
            }
            Dialogs.Add(new Dialog(InputDialog.DialogID, tmpList));
           
        }

        private void _CreateNodeList(XMLNode node)
        {
            DialogNodeList.Add(new DialogNode(node.name, node.text, node.NodeID));
        }
    }
}
