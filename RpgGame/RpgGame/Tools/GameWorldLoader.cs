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
using RpgGame.World;
using System.IO.Compression;
using GameWorld_Importer.XML_Objects;
using RpgGame.Manager;

namespace RpgGame.Tools
{
    class GameWorldLoader
    {
        public delegate void    TilesetDelegate        (String Value,ref Texture2D TilesetGraphic);
        public delegate void    TileDelegate           (String Value,ref Texture2D TileGraphic);
        public delegate void    ObjectDelegate         (String Value,ref GameObject Obj);
        public delegate void    LayerDelegate          (String Value,ref Layer Lyr);
        public delegate void    ObjectGroupDelegate    (String Value,ref Layer Lyr);
        public delegate void    WorldDelegate          (String Value,ref GameWorld Wrld);

        public      GameWorld           World       { get; private set; }
        public      int                 Spacing     { get; private set; }
        public      int                 Margin      { get; private set; }

        private     List<Texture2D>     Tilesets    { get; set; }
        private     List<Texture2D>     Tiles       { get; set; }

        public   static   Dictionary<String, TilesetDelegate>         TilesetPropertyFunctions    { get; private set; }
        public   static   Dictionary<String, TileDelegate>            TilePropertyFunctions       { get; private set; }
        public   static   Dictionary<String, ObjectDelegate>          ObjectPropertyFunctions     { get; private set; }
        public   static   Dictionary<String, ObjectGroupDelegate>     ObjectGroupFunctions        { get; private set; }
        public   static   Dictionary<String, LayerDelegate>           LayerPropertyFunctions      { get; private set; }
        public   static   Dictionary<String, WorldDelegate>           WorldPropertyFunction       { get; private set; }

        private     GameWorld   _World;

        public static void Initialize()
        {
            TilesetPropertyFunctions    =   new Dictionary<String, TilesetDelegate>();
            TilePropertyFunctions       =   new Dictionary<String, TileDelegate>();
            ObjectPropertyFunctions     =   new Dictionary<String, ObjectDelegate>();
            ObjectGroupFunctions        =   new Dictionary<String, ObjectGroupDelegate>();
            LayerPropertyFunctions      =   new Dictionary<String, LayerDelegate>();
            WorldPropertyFunction       =   new Dictionary<String, WorldDelegate>();
        }

        public GameWorldLoader()
        {
            _World      =  null;
            Tilesets    =  new List<Texture2D>();
            Tiles       =  new List<Texture2D>();
        }

        public GameWorldLoader(string filename) : this()
        {
            LoadTMX(filename);
        }

        public void LoadTMX(string filename)
        {
            XMLMap MapDescription = RpgGame.ContentManager.Load<XMLMap>(filename);

            _World = new GameWorld();

            _World.Name          = Path.GetFileName(filename);
            _World.Height        = MapDescription.Height;
            _World.Width         = MapDescription.Width;
            _World.PixelHeight   = MapDescription.Height * MapDescription.TileHeight;
            _World.PixelWidth    = MapDescription.Width * MapDescription.TileWidth;
            _World.TileHeight    = MapDescription.TileHeight;
            _World.TileWidth     = MapDescription.TileWidth;
            
            foreach(XMLProperty Property in MapDescription.Properties)
            {
                WorldDelegate PropertyFunction = null;
                if (WorldPropertyFunction.TryGetValue(Property.Name, out PropertyFunction))
                {
                    PropertyFunction(Property.Value, ref _World);
                }
            }

            foreach(XMLTileSet Tileset in MapDescription.TileSets)
            {
                _CreateTileset(Tileset);
            }

            foreach(XMLLayer Layer in MapDescription.Layers)
            {
                _CreateLayer(Layer);
            }

            foreach(XMLObjectGroup Objects in MapDescription.ObjectGroups)
            {
                _CreateObjects(Objects);
            }

            World = _World;

            //Release all tiles
            foreach (Texture2D Tex in Tiles)
            {
                Tex.Dispose();
            }

            //Sort layer based on their level
            World.Layers.Sort(delegate(Layer l1,Layer l2){return l1.Level - l2.Level;});
        }

        private void _CreateTileset(XMLTileSet InputTileset)
        {
            //Load the tileset texture
            string File = Path.GetFileNameWithoutExtension(InputTileset.Image.ImageSource);
            File = "Tilesets\\" + File;
            Texture2D TilesetGraphic = RpgGame.ContentManager.Load<Texture2D>(File);

            //Get the transparent color of the image
            Color TransparentColor = Color.Transparent;
            if (InputTileset.Image.Transparent.Length >= 6)
            {
                int Transparent_R = Convert.ToInt32(InputTileset.Image.Transparent.Substring(0, 2),16);
                int Transparent_G = Convert.ToInt32(InputTileset.Image.Transparent.Substring(2, 2),16);
                int Transparent_B = Convert.ToInt32(InputTileset.Image.Transparent.Substring(4, 2),16);

                TransparentColor = new Color(Transparent_R, Transparent_G, Transparent_B);
            }

            //if a Transparent color has been set, go through all pixels and
            //set all pixels transparent that match the transparent color
            if (TransparentColor != Color.Transparent)
            {
                //Set all Pixels transparent that match the transparent color
                Color[] GraphicPixels = new Color[TilesetGraphic.Width * TilesetGraphic.Height];
                TilesetGraphic.GetData<Color>(GraphicPixels);
                for (int i = 0; i < GraphicPixels.Length; ++i)
                {
                    if (GraphicPixels[i] == TransparentColor)
                    {
                        GraphicPixels[i] = Color.Transparent;
                    }
                }
                TilesetGraphic.SetData<Color>(GraphicPixels);
            }

            //Check if tileset has custom properties
            foreach (XMLProperty Property in InputTileset.Properties)
            {
                TilesetDelegate Function = null;
                if (TilesetPropertyFunctions.TryGetValue(Property.Name, out Function))
                {
                    Function(Property.Value, ref TilesetGraphic);
                }
            }

            //Crop each tile out of the previously loaded tileset graphic
            Texture2D Tile = null;
            
            for (int y = 0; y < TilesetGraphic.Height; y += _World.TileHeight)
            {
                for (int x = 0; x < TilesetGraphic.Width; x += _World.TileWidth)
                {
                    //Crop tile and add it to the list of tiles.
                    Rectangle CropRect = new Rectangle(x,y,_World.TileWidth,_World.TileHeight);
                    Tile = Texture2DCropper.Crop(TilesetGraphic,CropRect);
                    Tiles.Add(Tile);
                }
            }

            //Check if certain tiles have custom properties
            foreach(XMLTile XTile in InputTileset.Tiles)
            {
                foreach(XMLProperty Property in XTile.Properties)
                {
                    //Call custom function for each property
                    TileDelegate Function = null;
                    if(TilePropertyFunctions.TryGetValue(Property.Name,out Function))
                    {
                        int RealID = (XTile.ID + InputTileset.FirstGID) - 1;
                        Tile = Tiles.ElementAt<Texture2D>(RealID);
                        Function(Property.Value,ref Tile);
                    }
                }
            }          
        }

        private void _CreateLayer(XMLLayer InputLayer)
        {
            Layer NewLayer = _CheckForExistingLayer(InputLayer.Name);

            if(NewLayer == null){
                NewLayer = new Layer(_World);
            }

            NewLayer.Name = InputLayer.Name;

            RenderTarget2D LayerGraphic = new RenderTarget2D(GraphicSettings.GraphicDevice,
                                          InputLayer.Width * _World.TileWidth, InputLayer.Height * _World.TileHeight);

            GraphicSettings.GraphicDevice.SetRenderTarget(LayerGraphic);
            GraphicSettings.GraphicDevice.Clear(Color.Transparent);

            SpriteBatch Batch = new SpriteBatch(GraphicSettings.GraphicDevice);
            
            InputLayer.Data = InputLayer.Data.Replace("\n",String.Empty);
            InputLayer.Data = InputLayer.Data.Replace(" ",String.Empty);

            byte[] DecodedData = Convert.FromBase64String(InputLayer.Data);

            //Create a new memorystream in which we write the compressed data from the layer.
            MemoryStream Stream = new MemoryStream();
            Stream.Write(DecodedData,0,DecodedData.Length);
            Stream.Position = 0;

            int PosX = 0;
            int PosY = 0;

            //We need to decompress the data using the deflate stream
            GZipStream EncoderStream = new GZipStream(Stream,CompressionMode.Decompress);

            //Begin to draw to the layer graphic
            Batch.Begin();

            //Read decompressed data.
            BinaryReader Reader = new BinaryReader(EncoderStream);
            for(int i = 0;;++i){
                try{
                    uint SingleData = Reader.ReadUInt32();
                    if(SingleData != 0){
                        //Get tile based on read data and draw it to the layer graphic
                        Texture2D Tile = Tiles.ElementAt<Texture2D>((int)(SingleData - 1));
                        Batch.Draw(Tile, new Vector2(PosX, PosY), Color.White);

                        //Add the tile coordinates to the layer boundaries for collision detection
                        NewLayer.Boundaries.Add(new Rectangle(PosX, PosY, _World.TileWidth, _World.TileHeight));
                    }

                    //Increment x position because we're processing from left to right and top to bottom
                    PosX += _World.TileWidth;

                    //if the x position is outside the world, reset the x position and increment the y position
                    if (PosX >= _World.PixelWidth)
                    {
                        PosX = 0;
                        PosY += _World.TileHeight;
                    }
                }catch(EndOfStreamException ex){
                    break;
                } 
            }

            //Stop to draw after all data has been processed
            Batch.End();

            //Set the backbuffer as render target
            GraphicSettings.GraphicDevice.SetRenderTarget(null);

            //Close DeflateStream
            EncoderStream.Close();

            //Close memory stream
            Stream.Close();

            NewLayer.LayerGraphic = (Texture2D)LayerGraphic;

            //Go through each property of the layer and call associated functions
            foreach(XMLProperty Property in InputLayer.Properties)
            {
                if(Property.Name == "level"){
                    NewLayer.Level = Convert.ToInt32(Property.Value);
                }else{
                    LayerDelegate Function = null;
                    if (LayerPropertyFunctions.TryGetValue(Property.Name, out Function))
                    {
                        Function(Property.Value, ref NewLayer);
                    }
                }

            }

            //Add the layer to the world
            _World.Layers.Add(NewLayer);
        }

        private void _CreateObjects(XMLObjectGroup Objects)
        {
            Layer NewLayer = _CheckForExistingLayer(Objects.Name);
            
            if(NewLayer == null){
                NewLayer = new Layer(_World);
            }

            //Go through each property of the layer and call associated functions
            foreach(XMLProperty Property in Objects.Properties){
                ObjectGroupDelegate Function = null;
                if(ObjectGroupFunctions.TryGetValue(Property.Name,out Function)){
                    Function(Property.Value,ref NewLayer);
                }
            }

            //Go through all objects in the current object group
            foreach(XMLObject Obj in Objects.Objects){
                GameObject CurrentObj = new GameObject();
                
                //Set general object data
                float x = Obj.X;
                float y = Obj.Y;

                CurrentObj.Width    = Obj.Width;
                CurrentObj.Height   = Obj.Height;

                //Check if the object is associated to a tile graphic
                if(Obj.GID != -1){
                    //If the object has a tile graphic, we need to substract 1 tileheight from the y position
                    //due to a bug in the map editor
                    y -= _World.TileHeight;
                    
                    Texture2D TileGraphic = Tiles.ElementAt<Texture2D>(Obj.GID - 1);
                    Texture2D NewTexture = new Texture2D(TileGraphic.GraphicsDevice,TileGraphic.Width,TileGraphic.Height);
                    Color[] TileGraphicPixels = new Color[TileGraphic.Width * TileGraphic.Height];
                    TileGraphic.GetData<Color>(TileGraphicPixels);
                    NewTexture.SetData<Color>(TileGraphicPixels);
                    CurrentObj.AddComponent(new RenderableComponent(NewTexture));
                }

                //Set the final position of the object.
                CurrentObj.Position = new Vector2(x,y);

                //Go through each property of the object.
                foreach(XMLProperty Property in Obj.Properties){
                    ObjectDelegate Function = null;
                    if(ObjectPropertyFunctions.TryGetValue(Property.Name,out Function)){
                        Function(Property.Value,ref CurrentObj);
                    }
                }

                NewLayer.Objects.Add(CurrentObj);
            }

            _World.Layers.Add(NewLayer);
        }

        private Layer _CheckForExistingLayer(string LayerName)
        {
            Layer ReturnLayer = null;

            //Check whether the world already has a layer with the given name
            for(int i = 0;i < _World.Layers.Count; ++i){
                if(_World.Layers.ElementAt<Layer>(i).Name == LayerName){

                    //If a layer with the name has been found, remove it and return it.
                    ReturnLayer = _World.Layers.ElementAt<Layer>(i);
                    _World.Layers.Remove(ReturnLayer);
                    break;
                }
            }

            return ReturnLayer;
        }
    }
}
