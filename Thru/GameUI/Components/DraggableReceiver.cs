﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System;

namespace Thru
{
    public class DraggableReceiver
    {

        public Vector2 ScreenXY;
        public Rectangle Bounds;
        public Texture2D icon;

        public DraggableReceiver()
        {

        }

        public GameState Update(GameTime gameTime)
        {


            //if(mx)
            return GameState.Inventory;
        }

        public void Draw(SpriteBatch spriteBatch)
        {

        }
    }


}