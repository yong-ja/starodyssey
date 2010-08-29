namespace AvengersUtd.Odyssey.Graphics.Rendering
{
    /// <summary>
    /// To render the UI in the right way, it has to be rendered from back to front.
    /// This struct helps the Hud to know how many vertices and sprites to render
    /// before stopping. This enables the UI to correctly handle circumstances in
    /// which certain elements should occlude others. 
    /// </summary>
    public struct RenderStep
    {
        #region Properties

        /// <summary>
        /// Returns the number of vertices to render.
        /// </summary>
        public int VertexCount { get; set; }

        /// <summary>
        /// Returns the array index for the first index to use when rendering.
        /// </summary>
        public int BaseIndex { get; set; }

        /// <summary>
        /// Returns the array index for the first vertex to use when rendering.
        /// </summary>
        public int BaseVertex { get; set; }

        /// <summary>
        /// Returns the number of primitives the vertices form.
        /// </summary>
        public int PrimitiveCount { get; set; }

        /// <summary>
        /// Returns the array index for the first label to render.
        /// </summary>
        public int BaseLabelIndex { get; set; }

        /// <summary>
        /// Returns the number of labels to render.
        /// </summary>
        public int LabelCount { get; set; }

        #endregion

    }
}