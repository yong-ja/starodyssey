namespace AvengersUtd.Odyssey.Geometry
{
    /// <summary>
    /// An axis aligned bounding box.
    /// </summary>
    public struct AABB
    {
        /// <summary>
        /// The lower vertex
        /// </summary>
        public Vector2D LowerBound;

        /// <summary>
        /// The upper vertex
        /// </summary>
        public Vector2D UpperBound;

        public AABB(Vector2D min, Vector2D max)
            : this(ref min, ref max)
        {
        }

        public AABB(ref Vector2D min, ref Vector2D max)
        {
            LowerBound = min;
            UpperBound = max;
        }

        public AABB(Vector2D center, double width, double height)
        {
            LowerBound = center - new Vector2D(width / 2, height / 2);
            UpperBound = center + new Vector2D(width / 2, height / 2);
        }

        /// <summary>
        /// Get the center of the AABB.
        /// </summary>
        /// <value></value>
        public Vector2D Center
        {
            get { return 0.5f * (LowerBound + UpperBound); }
        }

        /// <summary>
        /// Get the extents of the AABB (half-widths).
        /// </summary>
        /// <value></value>
        public Vector2D Extents
        {
            get { return 0.5f * (UpperBound - LowerBound); }
        }

        /// <summary>
        /// Get the perimeter length
        /// </summary>
        /// <value></value>
        public double Perimeter
        {
            get
            {
                double wx = UpperBound.X - LowerBound.X;
                double wy = UpperBound.Y - LowerBound.Y;
                return 2.0f * (wx + wy);
            }
        }

        /// <summary>
        /// Gets the Vertices of the AABB.
        /// </summary>
        /// <value>The corners of the AABB</value>
        public Vertices Vertices
        {
            get
            {
                Vertices vertices = new Vertices
                                                            {
                                                                LowerBound,
                                                                new Vector2D(LowerBound.X, UpperBound.Y),
                                                                UpperBound,
                                                                new Vector2D(UpperBound.X, LowerBound.Y)
                                                            };
                return vertices;
            }
        }

        /// <summary>
        /// first quadrant
        /// </summary>
        public AABB Q1
        {
            get { return new AABB(Center, UpperBound); }
        }

        public AABB Q2
        {
            get
            {
                return new AABB(new Vector2D(LowerBound.X, Center.Y), new Vector2D(Center.X, UpperBound.Y));
            }
        }

        public AABB Q3
        {
            get { return new AABB(LowerBound, Center); }
        }

        public AABB Q4
        {
            get { return new AABB(new Vector2D(Center.X, LowerBound.Y), new Vector2D(UpperBound.X, Center.Y)); }
        }

        public Vector2D[] GetVerticesCollection()
        {
            Vector2D p1 = UpperBound;
            Vector2D p2 = new Vector2D(UpperBound.X, LowerBound.Y);
            Vector2D p3 = LowerBound;
            Vector2D p4 = new Vector2D(LowerBound.X, UpperBound.Y);
            return new[] { p1, p2, p3, p4 };
        }

        /// <summary>
        /// Verify that the bounds are sorted.
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if this instance is valid; otherwise, <c>false</c>.
        /// </returns>
        public bool IsValid()
        {
            Vector2D d = UpperBound - LowerBound;
            bool valid = d.X >= 0.0f && d.Y >= 0.0f;
            valid = valid && LowerBound.IsValid() && UpperBound.IsValid();
            return valid;
        }

        /// <summary>
        /// Combine an AABB into this one.
        /// </summary>
        /// <param name="aabb">The aabb.</param>
        public void Combine(ref AABB aabb)
        {
            LowerBound = Vector2D.Min(LowerBound, aabb.LowerBound);
            UpperBound = Vector2D.Max(UpperBound, aabb.UpperBound);
        }

        /// <summary>
        /// Combine two AABBs into this one.
        /// </summary>
        /// <param name="aabb1">The aabb1.</param>
        /// <param name="aabb2">The aabb2.</param>
        public void Combine(ref AABB aabb1, ref AABB aabb2)
        {
            LowerBound = Vector2D.Min(aabb1.LowerBound, aabb2.LowerBound);
            UpperBound = Vector2D.Max(aabb1.UpperBound, aabb2.UpperBound);
        }

        /// <summary>
        /// Does this aabb contain the provided AABB.
        /// </summary>
        /// <param name="aabb">The aabb.</param>
        /// <returns>
        /// 	<c>true</c> if it contains the specified aabb; otherwise, <c>false</c>.
        /// </returns>
        public bool Contains(ref AABB aabb)
        {
            bool result = LowerBound.X <= aabb.LowerBound.X;
            result = result && LowerBound.Y <= aabb.LowerBound.Y;
            result = result && aabb.UpperBound.X <= UpperBound.X;
            result = result && aabb.UpperBound.Y <= UpperBound.Y;
            return result;
        }

        /// <summary>
        /// Determines whether the AAABB contains the specified point.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <returns>
        /// 	<c>true</c> if it contains the specified point; otherwise, <c>false</c>.
        /// </returns>
        public bool Contains(ref Vector2D point)
        {
            //using epsilon to try and gaurd against double rounding errors.
            if ((point.X > (LowerBound.X + MathHelper.EpsilonD) && point.X < (UpperBound.X - MathHelper.EpsilonD) &&
                 (point.Y > (LowerBound.Y + MathHelper.EpsilonD) && point.Y < (UpperBound.Y - MathHelper.EpsilonD))))
            {
                return true;
            }
            return false;
        }

        public static bool TestOverlap(AABB a, AABB b)
        {
            return TestOverlap(ref a, ref b);
        }

        public static bool TestOverlap(ref AABB a, ref AABB b)
        {
            Vector2D d1 = b.LowerBound - a.UpperBound;
            Vector2D d2 = a.LowerBound - b.UpperBound;

            if (d1.X > 0.0f || d1.Y > 0.0f)
                return false;

            if (d2.X > 0.0f || d2.Y > 0.0f)
                return false;

            return true;
        }

        /*
        public static bool TestOverlap(Shape shapeA, int indexA,
                                       Shape shapeB, int indexB,
                                       ref Transform xfA, ref Transform xfB)
        {
            _input.ProxyA.Set(shapeA, indexA);
            _input.ProxyB.Set(shapeB, indexB);
            _input.TransformA = xfA;
            _input.TransformB = xfB;
            _input.UseRadii = true;

            SimplexCache cache;
            DistanceOutput output;
            Distance.ComputeDistance(out output, out cache, _input);

            return output.Distance < 10.0f * Settings.Epsilon;
        }


        // From Real-time Collision Detection, p179.
        public bool RayCast(out RayCastOutput output, ref RayCastInput input)
        {
            output = new RayCastOutput();

            double tmin = -Settings.Maxdouble;
            double tmax = Settings.Maxdouble;

            Vector2D p = input.Point1;
            Vector2D d = input.Point2 - input.Point1;
            Vector2D absD = MathUtils.Abs(d);

            Vector2D normal = Vector2D.Zero;

            for (int i = 0; i < 2; ++i)
            {
                double absD_i = i == 0 ? absD.X : absD.Y;
                double lowerBound_i = i == 0 ? LowerBound.X : LowerBound.Y;
                double upperBound_i = i == 0 ? UpperBound.X : UpperBound.Y;
                double p_i = i == 0 ? p.X : p.Y;

                if (absD_i < Settings.Epsilon)
                {
                    // Parallel.
                    if (p_i < lowerBound_i || upperBound_i < p_i)
                    {
                        return false;
                    }
                }
                else
                {
                    double d_i = i == 0 ? d.X : d.Y;

                    double inv_d = 1.0f / d_i;
                    double t1 = (lowerBound_i - p_i) * inv_d;
                    double t2 = (upperBound_i - p_i) * inv_d;

                    // Sign of the normal vector.
                    double s = -1.0f;

                    if (t1 > t2)
                    {
                        MathUtils.Swap(ref t1, ref t2);
                        s = 1.0f;
                    }

                    // Push the min up
                    if (t1 > tmin)
                    {
                        if (i == 0)
                        {
                            normal.X = s;
                        }
                        else
                        {
                            normal.Y = s;
                        }

                        tmin = t1;
                    }

                    // Pull the max down
                    tmax = Math.Min(tmax, t2);

                    if (tmin > tmax)
                    {
                        return false;
                    }
                }
            }

            // Does the ray start inside the box?
            // Does the ray intersect beyond the max fraction?
            if (tmin < 0.0f || input.MaxFraction < tmin)
            {
                return false;
            }

            // Intersection.
            output.Fraction = tmin;
            output.Normal = normal;
            return true;
        }
         * 
         * */
    }
}
