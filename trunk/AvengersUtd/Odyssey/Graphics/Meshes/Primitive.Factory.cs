namespace AvengersUtd.Odyssey.Graphics.Meshes
{
    public static class Primitive
    {
        public static Sphere CreateSphere(float radius, int numStrips)
        {
            Sphere sphere = new Sphere(radius, numStrips);
            sphere.Init();
            return sphere;
        }

        
    }
}
