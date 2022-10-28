namespace DuckGame
{
    internal class w : Thing // shshs
    {
        public w()
          : base()
        {
            layer = Layer.HUD;
        }

        public override void Update()
        {
        }

        public override void Draw()
        {
            Camera camera = Layer.HUD.camera;
            Graphics.DrawRect(camera.position + camera.size - new Vec2(10f, 12f), camera.position + camera.size - new Vec2(9f, 11f), Color.Black);
        }
    }
}
