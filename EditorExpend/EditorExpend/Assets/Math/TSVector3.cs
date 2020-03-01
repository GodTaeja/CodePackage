/* Copyright (C) <2009-2011> <Thorben Linneweber, Jitter Physics>
* 
*  This software is provided 'as-is', without any express or implied
*  warranty.  In no event will the authors be held liable for any damages
*  arising from the use of this software.
*
*  Permission is granted to anyone to use this software for any purpose,
*  including commercial applications, and to alter it and redistribute it
*  freely, subject to the following restrictions:
*
*  1. The origin of this software must not be misrepresented; you must not
*      claim that you wrote the original software. If you use this software
*      in a product, an acknowledgment in the product documentation would be
*      appreciated but is not required.
*  2. Altered source versions must be plainly marked as such, and must not be
*      misrepresented as being the original software.
*  3. This notice may not be removed or altered from any source distribution. 
*/

using System;

#if INLINE_ENABLE
using System.Runtime.CompilerServices;
#endif

namespace TrueSync
{
    /// <summary>
    /// A vector structure.
    /// </summary>
    [Serializable]
    public struct TSVector3
    {

        private static FP ZeroEpsilonSq = TSMath.Epsilon;
        internal static TSVector3 InternalZero;
        internal static TSVector3 Arbitrary;

        /// <summary>The X component of the vector.</summary>
        public FP x;
        /// <summary>The Y component of the vector.</summary>
        public FP y;
        /// <summary>The Z component of the vector.</summary>
        public FP z;

        #region Static readonly variables
        /// <summary>
        /// A vector with components (0,0,0);
        /// </summary>
        public static readonly TSVector3 zero;
        /// <summary>
        /// A vector with components (-1,0,0);
        /// </summary>
        public static readonly TSVector3 left;
        /// <summary>
        /// A vector with components (1,0,0);
        /// </summary>
        public static readonly TSVector3 right;
        /// <summary>
        /// A vector with components (0,1,0);
        /// </summary>
        public static readonly TSVector3 up;
        /// <summary>
        /// A vector with components (0,-1,0);
        /// </summary>
        public static readonly TSVector3 down;
        /// <summary>
        /// A vector with components (0,0,-1);
        /// </summary>
        public static readonly TSVector3 back;
        /// <summary>
        /// A vector with components (0,0,1);
        /// </summary>
        public static readonly TSVector3 forward;
        /// <summary>
        /// A vector with components (1,1,1);
        /// </summary>
        public static readonly TSVector3 one;
        /// <summary>
        /// A vector with components 
        /// (FP.MinValue,FP.MinValue,FP.MinValue);
        /// </summary>
        public static readonly TSVector3 MinValue;
        /// <summary>
        /// A vector with components 
        /// (FP.MaxValue,FP.MaxValue,FP.MaxValue);
        /// </summary>
        public static readonly TSVector3 MaxValue;
        #endregion

        #region Private static constructor
        static TSVector3()
        {
            one = new TSVector3(1, 1, 1);
            zero = new TSVector3(0, 0, 0);
            left = new TSVector3(-1, 0, 0);
            right = new TSVector3(1, 0, 0);
            up = new TSVector3(0, 1, 0);
            down = new TSVector3(0, -1, 0);
            back = new TSVector3(0, 0, -1);
            forward = new TSVector3(0, 0, 1);
            MinValue = new TSVector3(FP.MinValue);
            MaxValue = new TSVector3(FP.MaxValue);
            Arbitrary = new TSVector3(1, 1, 1);
            InternalZero = zero;
        }
        #endregion

#if INLINE_ENABLE
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static TSVector3 Abs(TSVector3 other)
        {
            return new TSVector3(FP.Abs(other.x), FP.Abs(other.y), FP.Abs(other.z));
        }

        /// <summary>
        /// Gets the squared length of the vector.
        /// </summary>
        /// <returns>Returns the squared length of the vector.</returns>
        public FP sqrMagnitude
        {
#if INLINE_ENABLE
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
            get
            {
                return (((this.x * this.x) + (this.y * this.y)) + (this.z * this.z));
            }
        }

        public FP sqrMagnitude2D
        {
#if INLINE_ENABLE
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
            get
            {
                return ((this.x * this.x) + (this.z * this.z));
            }
        }

        /// <summary>
        /// Gets the length of the vector.
        /// </summary>
        /// <returns>Returns the length of the vector.</returns>
        public FP magnitude
        {
#if INLINE_ENABLE
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
            get
            {
                 FP num = ((this.x * this.x) + (this.y * this.y)) + (this.z * this.z);
                return FP.Sqrt(num);
            }
        }

        public FP magnitude2D
        {
#if INLINE_ENABLE
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
            get
            {
                FP num = (this.x * this.x) + (this.z * this.z);
                return FP.Sqrt(num);
            }
        }

#if INLINE_ENABLE
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static TSVector3 ClampMagnitude(TSVector3 vector, FP maxLength)
        {
            if (vector.magnitude <= maxLength)
            {
                return vector;
            }

            return Normalize(vector) * maxLength;
        }

        /// <summary>
        /// Gets a normalized version of the vector.
        /// </summary>
        /// <returns>Returns a normalized version of the vector.</returns>
        public TSVector3 normalized
        {
#if INLINE_ENABLE
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
            get
            {
                TSVector3 result = new TSVector3(this.x, this.y, this.z);
                result.Normalize();

                return result;
            }
        }

        /// <summary>
        /// Constructor initializing a new instance of the structure
        /// </summary>
        /// <param name="x">The X component of the vector.</param>
        /// <param name="y">The Y component of the vector.</param>
        /// <param name="z">The Z component of the vector.</param>

#if INLINE_ENABLE
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public TSVector3(int x, int y, int z)
        {
            this.x = (FP)x;
            this.y = (FP)y;
            this.z = (FP)z;
        }

#if INLINE_ENABLE
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public TSVector3(FP x, FP y, FP z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        /// <summary>
        /// Multiplies each component of the vector by the same components of the provided vector.
        /// </summary>
#if INLINE_ENABLE
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public void Scale(TSVector3 other)
        {
            this.x = x * other.x;
            this.y = y * other.y;
            this.z = z * other.z;
        }

        /// <summary>
        /// Sets all vector component to specific values.
        /// </summary>
        /// <param name="x">The X component of the vector.</param>
        /// <param name="y">The Y component of the vector.</param>
        /// <param name="z">The Z component of the vector.</param>
#if INLINE_ENABLE
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public void Set(FP x, FP y, FP z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        /// <summary>
        /// Constructor initializing a new instance of the structure
        /// </summary>
        /// <param name="xyz">All components of the vector are set to xyz</param>
#if INLINE_ENABLE
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public TSVector3(FP xyz)
        {
            this.x = xyz;
            this.y = xyz;
            this.z = xyz;
        }

#if INLINE_ENABLE
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static TSVector3 Lerp(TSVector3 from, TSVector3 to, FP percent)
        {
            return from + (to - from) * percent;
        }

        /// <summary>
        /// Builds a string from the JVector.
        /// </summary>
        /// <returns>A string containing all three components.</returns>
#region public override string ToString()
        public override string ToString()
        {
            return string.Format("({0:f6}, {1:f6}, {2:f6})", x.AsFloat(), y.AsFloat(), z.AsFloat());
        }

        public string ToRawString()
        {
            return string.Format("({0}, {1}, {2})", x.RawValue, y.RawValue, z.RawValue);
        }
#endregion

        /// <summary>
        /// Tests if an object is equal to this vector.
        /// </summary>
        /// <param name="obj">The object to test.</param>
        /// <returns>Returns true if they are euqal, otherwise false.</returns>
#region public override bool Equals(object obj)
        public override bool Equals(object obj)
        {
            if (!(obj is TSVector3)) return false;
            TSVector3 other = (TSVector3)obj;

            return (((x == other.x) && (y == other.y)) && (z == other.z));
        }
        #endregion

        /// <summary>
        /// Multiplies each component of the vector by the same components of the provided vector.
        /// </summary>
#if INLINE_ENABLE
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static TSVector3 Scale(TSVector3 vecA, TSVector3 vecB)
        {
            TSVector3 result;
            result.x = vecA.x * vecB.x;
            result.y = vecA.y * vecB.y;
            result.z = vecA.z * vecB.z;

            return result;
        }

        /// <summary>
        /// Tests if two JVector are equal.
        /// </summary>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        /// <returns>Returns true if both values are equal, otherwise false.</returns>
#region public static bool operator ==(JVector value1, JVector value2)
        public static bool operator ==(TSVector3 value1, TSVector3 value2)
        {
            return (((value1.x == value2.x) && (value1.y == value2.y)) && (value1.z == value2.z));
        }
#endregion

        /// <summary>
        /// Tests if two JVector are not equal.
        /// </summary>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        /// <returns>Returns false if both values are equal, otherwise true.</returns>
#region public static bool operator !=(JVector value1, JVector value2)
        public static bool operator !=(TSVector3 value1, TSVector3 value2)
        {
            if ((value1.x == value2.x) && (value1.y == value2.y))
            {
                return (value1.z != value2.z);
            }
            return true;
        }
#endregion

        /// <summary>
        /// Gets a vector with the minimum x,y and z values of both vectors.
        /// </summary>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        /// <returns>A vector with the minimum x,y and z values of both vectors.</returns>
#region public static JVector Min(JVector value1, JVector value2)

        public static TSVector3 Min(TSVector3 value1, TSVector3 value2)
        {
            TSVector3 result;
            TSVector3.Min(ref value1, ref value2, out result);
            return result;
        }

        /// <summary>
        /// Gets a vector with the minimum x,y and z values of both vectors.
        /// </summary>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        /// <param name="result">A vector with the minimum x,y and z values of both vectors.</param>
#if INLINE_ENABLE
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static void Min(ref TSVector3 value1, ref TSVector3 value2, out TSVector3 result)
        {
            result.x = (value1.x < value2.x) ? value1.x : value2.x;
            result.y = (value1.y < value2.y) ? value1.y : value2.y;
            result.z = (value1.z < value2.z) ? value1.z : value2.z;
        }
        #endregion

        /// <summary>
        /// Gets a vector with the maximum x,y and z values of both vectors.
        /// </summary>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        /// <returns>A vector with the maximum x,y and z values of both vectors.</returns>
        #region public static JVector Max(JVector value1, JVector value2)
#if INLINE_ENABLE
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static TSVector3 Max(TSVector3 value1, TSVector3 value2)
        {
            TSVector3 result;
            TSVector3.Max(ref value1, ref value2, out result);
            return result;
        }

#if INLINE_ENABLE
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static FP Distance(TSVector3 v1, TSVector3 v2)
        {
            return FP.Sqrt((v1.x - v2.x) * (v1.x - v2.x) + (v1.y - v2.y) * (v1.y - v2.y) + (v1.z - v2.z) * (v1.z - v2.z));
        }

#if INLINE_ENABLE
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static FP Distance2D(TSVector3 v1, TSVector3 v2)
        {
            return FP.Sqrt((v1.x - v2.x) * (v1.x - v2.x) + (v1.z - v2.z) * (v1.z - v2.z));
        }

        /// <summary>
        /// Gets a vector with the maximum x,y and z values of both vectors.
        /// </summary>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        /// <param name="result">A vector with the maximum x,y and z values of both vectors.</param>
#if INLINE_ENABLE
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static void Max(ref TSVector3 value1, ref TSVector3 value2, out TSVector3 result)
        {
            result.x = (value1.x > value2.x) ? value1.x : value2.x;
            result.y = (value1.y > value2.y) ? value1.y : value2.y;
            result.z = (value1.z > value2.z) ? value1.z : value2.z;
        }
        #endregion

        /// <summary>
        /// Sets the length of the vector to zero.
        /// </summary>
        #region public void MakeZero()
#if INLINE_ENABLE
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public void MakeZero()
        {
            x = FP.Zero;
            y = FP.Zero;
            z = FP.Zero;
        }
        #endregion

        /// <summary>
        /// Checks if the length of the vector is zero.
        /// </summary>
        /// <returns>Returns true if the vector is zero, otherwise false.</returns>
        #region public bool IsZero()
#if INLINE_ENABLE
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public bool IsZero()
        {
            return (this.sqrMagnitude == FP.Zero);
        }

        /// <summary>
        /// Checks if the length of the vector is nearly zero.
        /// </summary>
        /// <returns>Returns true if the vector is nearly zero, otherwise false.</returns>
        public bool IsNearlyZero()
        {
            return (this.sqrMagnitude < ZeroEpsilonSq);
        }
        #endregion

        /// <summary>
        /// Transforms a vector by the given matrix.
        /// </summary>
        /// <param name="position">The vector to transform.</param>
        /// <param name="matrix">The transform matrix.</param>
        /// <returns>The transformed vector.</returns>
        #region public static JVector Transform(JVector position, JMatrix matrix)
#if INLINE_ENABLE
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static TSVector3 Transform(TSVector3 position, TSMatrix matrix)
        {
            TSVector3 result;
            TSVector3.Transform(ref position, ref matrix, out result);
            return result;
        }

        /// <summary>
        /// Transforms a vector by the given matrix.
        /// </summary>
        /// <param name="position">The vector to transform.</param>
        /// <param name="matrix">The transform matrix.</param>
        /// <param name="result">The transformed vector.</param>
#if INLINE_ENABLE
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static void Transform(ref TSVector3 position, ref TSMatrix matrix, out TSVector3 result)
        {
            FP num0 = ((position.x * matrix.M11) + (position.y * matrix.M21)) + (position.z * matrix.M31);
            FP num1 = ((position.x * matrix.M12) + (position.y * matrix.M22)) + (position.z * matrix.M32);
            FP num2 = ((position.x * matrix.M13) + (position.y * matrix.M23)) + (position.z * matrix.M33);

            result.x = num0;
            result.y = num1;
            result.z = num2;
        }

        /// <summary>
        /// Transforms a vector by the transposed of the given Matrix.
        /// </summary>
        /// <param name="position">The vector to transform.</param>
        /// <param name="matrix">The transform matrix.</param>
        /// <param name="result">The transformed vector.</param>
#if INLINE_ENABLE
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static void TransposedTransform(ref TSVector3 position, ref TSMatrix matrix, out TSVector3 result)
        {
            FP num0 = ((position.x * matrix.M11) + (position.y * matrix.M12)) + (position.z * matrix.M13);
            FP num1 = ((position.x * matrix.M21) + (position.y * matrix.M22)) + (position.z * matrix.M23);
            FP num2 = ((position.x * matrix.M31) + (position.y * matrix.M32)) + (position.z * matrix.M33);

            result.x = num0;
            result.y = num1;
            result.z = num2;
        }
        #endregion

        /// <summary>
        /// Calculates the dot product of two vectors.
        /// </summary>
        /// <param name="vector1">The first vector.</param>
        /// <param name="vector2">The second vector.</param>
        /// <returns>Returns the dot product of both vectors.</returns>
        #region public static FP Dot(JVector vector1, JVector vector2)
#if INLINE_ENABLE
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static FP Dot(TSVector3 vector1, TSVector3 vector2)
        {
            return TSVector3.Dot(ref vector1, ref vector2);
        }


        /// <summary>
        /// Calculates the dot product of both vectors.
        /// </summary>
        /// <param name="vector1">The first vector.</param>
        /// <param name="vector2">The second vector.</param>
        /// <returns>Returns the dot product of both vectors.</returns>
#if INLINE_ENABLE
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static FP Dot(ref TSVector3 vector1, ref TSVector3 vector2)
        {
            return ((vector1.x * vector2.x) + (vector1.y * vector2.y)) + (vector1.z * vector2.z);
        }
        #endregion

        // Projects a vector onto another vector.
#if INLINE_ENABLE
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static TSVector3 Project(TSVector3 vector, TSVector3 onNormal)
        {
            FP sqrtMag = Dot(onNormal, onNormal);
            if (sqrtMag < TSMath.Epsilon)
                return zero;
            else
                return onNormal * Dot(vector, onNormal) / sqrtMag;
        }

        // Projects a vector onto a plane defined by a normal orthogonal to the plane.
#if INLINE_ENABLE
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static TSVector3 ProjectOnPlane(TSVector3 vector, TSVector3 planeNormal)
        {
            return vector - Project(vector, planeNormal);
        }


        // Returns the angle in degrees between /from/ and /to/. This is always the smallest
#if INLINE_ENABLE
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static FP Angle(TSVector3 from, TSVector3 to)
        {
            return TSMath.Acos(TSMath.Clamp(Dot(from.normalized, to.normalized), -FP.ONE, FP.ONE)) * TSMath.Rad2Deg;
        }

        // The smaller of the two possible angles between the two vectors is returned, therefore the result will never be greater than 180 degrees or smaller than -180 degrees.
        // If you imagine the from and to vectors as lines on a piece of paper, both originating from the same point, then the /axis/ vector would point up out of the paper.
        // The measured angle between the two vectors would be positive in a clockwise direction and negative in an anti-clockwise direction.
#if INLINE_ENABLE
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static FP SignedAngle(TSVector3 from, TSVector3 to, TSVector3 axis)
        {
            TSVector3 fromNorm = from.normalized, toNorm = to.normalized;
            FP unsignedAngle = TSMath.Acos(TSMath.Clamp(Dot(fromNorm, toNorm), -FP.ONE, FP.ONE)) * TSMath.Rad2Deg;
            FP sign = TSMath.Sign(Dot(axis, Cross(fromNorm, toNorm)));
            return unsignedAngle * sign;
        }

        /// <summary>
        /// Adds two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <returns>The sum of both vectors.</returns>
#region public static void Add(JVector value1, JVector value2)
#if INLINE_ENABLE
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static TSVector3 Add(TSVector3 value1, TSVector3 value2)
        {
            TSVector3 result;
            TSVector3.Add(ref value1, ref value2, out result);
            return result;
        }

        /// <summary>
        /// Adds to vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <param name="result">The sum of both vectors.</param>
#if INLINE_ENABLE
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static void Add(ref TSVector3 value1, ref TSVector3 value2, out TSVector3 result)
        {
            FP num0 = value1.x + value2.x;
            FP num1 = value1.y + value2.y;
            FP num2 = value1.z + value2.z;

            result.x = num0;
            result.y = num1;
            result.z = num2;
        }
        #endregion

        /// <summary>
        /// Divides a vector by a factor.
        /// </summary>
        /// <param name="value1">The vector to divide.</param>
        /// <param name="scaleFactor">The scale factor.</param>
        /// <returns>Returns the scaled vector.</returns>
#if INLINE_ENABLE
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static TSVector3 Divide(TSVector3 value1, FP scaleFactor)
        {
            TSVector3 result;
            TSVector3.Divide(ref value1, scaleFactor, out result);
            return result;
        }

#if INLINE_ENABLE
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static TSVector3 Divide(TSVector3 value1, int scaleFactor)
        {
            TSVector3 result;
            TSVector3.Divide(ref value1, scaleFactor, out result);
            return result;
        }

#if INLINE_ENABLE
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static TSVector3 Divide(TSVector3 value1, long scaleFactor)
        {
            TSVector3 result;
            TSVector3.Divide(ref value1, scaleFactor, out result);
            return result;
        }

        /// <summary>
        /// Divides a vector by a factor.
        /// </summary>
        /// <param name="value1">The vector to divide.</param>
        /// <param name="scaleFactor">The scale factor.</param>
        /// <param name="result">Returns the scaled vector.</param>
#if INLINE_ENABLE
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static void Divide(ref TSVector3 value1, FP scaleFactor, out TSVector3 result)
        {
            result.x = value1.x / scaleFactor;
            result.y = value1.y / scaleFactor;
            result.z = value1.z / scaleFactor;
        }

#if INLINE_ENABLE
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static void Divide(ref TSVector3 value1, int scaleFactor, out TSVector3 result)
        {
            result.x = value1.x / scaleFactor;
            result.y = value1.y / scaleFactor;
            result.z = value1.z / scaleFactor;
        }

#if INLINE_ENABLE
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static void Divide(ref TSVector3 value1, long scaleFactor, out TSVector3 result)
        {
            result.x = value1.x / scaleFactor;
            result.y = value1.y / scaleFactor;
            result.z = value1.z / scaleFactor;
        }

        /// <summary>
        /// Subtracts two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <returns>The difference of both vectors.</returns>
#region public static JVector Subtract(JVector value1, JVector value2)
#if INLINE_ENABLE
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static TSVector3 Subtract(TSVector3 value1, TSVector3 value2)
        {
            TSVector3 result;
            TSVector3.Subtract(ref value1, ref value2, out result);
            return result;
        }

        /// <summary>
        /// Subtracts to vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <param name="result">The difference of both vectors.</param>
#if INLINE_ENABLE
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static void Subtract(ref TSVector3 value1, ref TSVector3 value2, out TSVector3 result)
        {
            FP num0 = value1.x - value2.x;
            FP num1 = value1.y - value2.y;
            FP num2 = value1.z - value2.z;

            result.x = num0;
            result.y = num1;
            result.z = num2;
        }
        #endregion

        /// <summary>
        /// The cross product of two vectors.
        /// </summary>
        /// <param name="vector1">The first vector.</param>
        /// <param name="vector2">The second vector.</param>
        /// <returns>The cross product of both vectors.</returns>
        #region public static JVector Cross(JVector vector1, JVector vector2)
#if INLINE_ENABLE
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static TSVector3 Cross(TSVector3 vector1, TSVector3 vector2)
        {
            TSVector3 result;
            TSVector3.Cross(ref vector1, ref vector2, out result);
            return result;
        }

        /// <summary>
        /// The cross product of two vectors.
        /// </summary>
        /// <param name="vector1">The first vector.</param>
        /// <param name="vector2">The second vector.</param>
        /// <param name="result">The cross product of both vectors.</param>
#if INLINE_ENABLE
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static void Cross(ref TSVector3 vector1, ref TSVector3 vector2, out TSVector3 result)
        {
            FP num3 = (vector1.y * vector2.z) - (vector1.z * vector2.y);
            FP num2 = (vector1.z * vector2.x) - (vector1.x * vector2.z);
            FP num = (vector1.x * vector2.y) - (vector1.y * vector2.x);
            result.x = num3;
            result.y = num2;
            result.z = num;
        }
        #endregion

        /// <summary>
        /// Gets the hashcode of the vector.
        /// </summary>
        /// <returns>Returns the hashcode of the vector.</returns>
        #region public override int GetHashCode()
        public override int GetHashCode()
        {
            return x.GetHashCode() ^ y.GetHashCode() ^ z.GetHashCode();
        }
        #endregion

        /// <summary>
        /// Inverses the direction of the vector.
        /// </summary>
        #region public static JVector Negate(JVector value)
#if INLINE_ENABLE
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public void Negate()
        {
            this.x = -this.x;
            this.y = -this.y;
            this.z = -this.z;
        }

        /// <summary>
        /// Inverses the direction of a vector.
        /// </summary>
        /// <param name="value">The vector to inverse.</param>
        /// <returns>The negated vector.</returns>
#if INLINE_ENABLE
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static TSVector3 Negate(TSVector3 value)
        {
            TSVector3 result;
            TSVector3.Negate(ref value, out result);
            return result;
        }

        /// <summary>
        /// Inverses the direction of a vector.
        /// </summary>
        /// <param name="value">The vector to inverse.</param>
        /// <param name="result">The negated vector.</param>
#if INLINE_ENABLE
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static void Negate(ref TSVector3 value, out TSVector3 result)
        {
            FP num0 = -value.x;
            FP num1 = -value.y;
            FP num2 = -value.z;

            result.x = num0;
            result.y = num1;
            result.z = num2;
        }
        #endregion

        /// <summary>
        /// Normalizes the given vector.
        /// </summary>
        /// <param name="value">The vector which should be normalized.</param>
        /// <returns>A normalized vector.</returns>
        #region public static JVector Normalize(JVector value)
#if INLINE_ENABLE
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static TSVector3 Normalize(TSVector3 value)
        {
            TSVector3 result;
            TSVector3.Normalize(ref value, out result);
            return result;
        }

#if INLINE_ENABLE
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static TSVector3 NormalizeCheck(TSVector3 v)
        {
            //防止溢出
            FP div = TSMath.Max(TSMath.Abs(v.x), TSMath.Abs(v.y), TSMath.Abs(v.z));
            return (v / div);
        }

        /// <summary>
        /// Normalizes this vector.
        /// </summary>
#if INLINE_ENABLE
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public void Normalize()
        {
            this = NormalizeCheck(this);


            FP num2 = ((this.x * this.x) + (this.y * this.y)) + (this.z * this.z);
            FP num = FP.One / FP.Sqrt(num2);
            this.x *= num;
            this.y *= num;
            this.z *= num;
        }

        /// <summary>
        /// Normalizes the given vector.
        /// </summary>
        /// <param name="value">The vector which should be normalized.</param>
        /// <param name="result">A normalized vector.</param>
#if INLINE_ENABLE
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static void Normalize(ref TSVector3 value, out TSVector3 result)
        {
            TSVector3 value1 = NormalizeCheck(value);

            FP num2 = ((value1.x * value1.x) + (value1.y * value1.y)) + (value1.z * value1.z);
            FP num = FP.One / FP.Sqrt(num2);
            result.x = value1.x * num;
            result.y = value1.y * num;
            result.z = value1.z * num;
        }
#endregion

#region public static void Swap(ref JVector vector1, ref JVector vector2)

        /// <summary>
        /// Swaps the components of both vectors.
        /// </summary>
        /// <param name="vector1">The first vector to swap with the second.</param>
        /// <param name="vector2">The second vector to swap with the first.</param>
#if INLINE_ENABLE
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static void Swap(ref TSVector3 vector1, ref TSVector3 vector2)
        {
            FP temp;

            temp = vector1.x;
            vector1.x = vector2.x;
            vector2.x = temp;

            temp = vector1.y;
            vector1.y = vector2.y;
            vector2.y = temp;

            temp = vector1.z;
            vector1.z = vector2.z;
            vector2.z = temp;
        }
        #endregion

        /// <summary>
        /// Multiply a vector with a factor.
        /// </summary>
        /// <param name="value1">The vector to multiply.</param>
        /// <param name="scaleFactor">The scale factor.</param>
        /// <returns>Returns the multiplied vector.</returns>
        #region public static JVector Multiply(JVector value1, FP scaleFactor)
#if INLINE_ENABLE
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static TSVector3 Multiply(TSVector3 value1, FP scaleFactor)
        {
            TSVector3 result;
            TSVector3.Multiply(ref value1, scaleFactor, out result);
            return result;
        }

#if INLINE_ENABLE
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static TSVector3 Multiply(TSVector3 value1, int scaleFactor)
        {
            TSVector3 result;
            TSVector3.Multiply(ref value1, scaleFactor, out result);
            return result;
        }

#if INLINE_ENABLE
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static TSVector3 Multiply(TSVector3 value1, long scaleFactor)
        {
            TSVector3 result;
            TSVector3.Multiply(ref value1, scaleFactor, out result);
            return result;
        }

        /// <summary>
        /// Multiply a vector with a factor.
        /// </summary>
        /// <param name="value1">The vector to multiply.</param>
        /// <param name="scaleFactor">The scale factor.</param>
        /// <param name="result">Returns the multiplied vector.</param>
#if INLINE_ENABLE
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static void Multiply(ref TSVector3 value1, FP scaleFactor, out TSVector3 result)
        {
            result.x = value1.x * scaleFactor;
            result.y = value1.y * scaleFactor;
            result.z = value1.z * scaleFactor;
        }

#if INLINE_ENABLE
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static void Multiply(ref TSVector3 value1, int scaleFactor, out TSVector3 result)
        {
            result.x = value1.x * scaleFactor;
            result.y = value1.y * scaleFactor;
            result.z = value1.z * scaleFactor;
        }

#if INLINE_ENABLE
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static void Multiply(ref TSVector3 value1, long scaleFactor, out TSVector3 result)
        {
            result.x = value1.x * scaleFactor;
            result.y = value1.y * scaleFactor;
            result.z = value1.z * scaleFactor;
        }
        #endregion

        /// <summary>
        /// Calculates the cross product of two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <returns>Returns the cross product of both.</returns>
        #region public static JVector operator %(JVector value1, JVector value2)
#if INLINE_ENABLE
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static TSVector3 operator %(TSVector3 value1, TSVector3 value2)
        {
            TSVector3 result; TSVector3.Cross(ref value1, ref value2, out result);
            return result;
        }
        #endregion

        /// <summary>
        /// Calculates the dot product of two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <returns>Returns the dot product of both.</returns>
        #region public static FP operator *(JVector value1, JVector value2)
#if INLINE_ENABLE
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static FP operator *(TSVector3 value1, TSVector3 value2)
        {
            return TSVector3.Dot(ref value1, ref value2);
        }
        #endregion

        /// <summary>
        /// Multiplies a vector by a scale factor.
        /// </summary>
        /// <param name="value1">The vector to scale.</param>
        /// <param name="value2">The scale factor.</param>
        /// <returns>Returns the scaled vector.</returns>
        #region public static JVector operator *(JVector value1, FP value2)
#if INLINE_ENABLE
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static TSVector3 operator *(TSVector3 value1, FP value2)
        {
            TSVector3 result;
            TSVector3.Multiply(ref value1, value2, out result);
            return result;
        }

#if INLINE_ENABLE
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static TSVector3 operator *(TSVector3 value1, int value2)
        {
            TSVector3 result;
            TSVector3.Multiply(ref value1, value2, out result);
            return result;
        }

#if INLINE_ENABLE
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static TSVector3 operator *(TSVector3 value1, long value2)
        {
            TSVector3 result;
            TSVector3.Multiply(ref value1, value2, out result);
            return result;
        }


        #endregion

        /// <summary>
        /// Multiplies a vector by a scale factor.
        /// </summary>
        /// <param name="value2">The vector to scale.</param>
        /// <param name="value1">The scale factor.</param>
        /// <returns>Returns the scaled vector.</returns>
        #region public static JVector operator *(FP value1, JVector value2)
#if INLINE_ENABLE
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static TSVector3 operator *(FP value1, TSVector3 value2)
        {
            TSVector3 result;
            TSVector3.Multiply(ref value2, value1, out result);
            return result;
        }

#if INLINE_ENABLE
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static TSVector3 operator *(int value1, TSVector3 value2)
        {
            TSVector3 result;
            TSVector3.Multiply(ref value2, value1, out result);
            return result;
        }

#if INLINE_ENABLE
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static TSVector3 operator *(long value1, TSVector3 value2)
        {
            TSVector3 result;
            TSVector3.Multiply(ref value2, value1, out result);
            return result;
        }
        #endregion

        /// <summary>
        /// Subtracts two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <returns>The difference of both vectors.</returns>
        #region public static JVector operator -(JVector value1, JVector value2)
#if INLINE_ENABLE
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static TSVector3 operator -(TSVector3 value1, TSVector3 value2)
        {
            TSVector3 result; TSVector3.Subtract(ref value1, ref value2, out result);
            return result;
        }
        #endregion

        /// <summary>
        /// Adds two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <returns>The sum of both vectors.</returns>
        #region public static JVector operator +(JVector value1, JVector value2)
#if INLINE_ENABLE
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static TSVector3 operator +(TSVector3 value1, TSVector3 value2)
        {
            TSVector3 result; TSVector3.Add(ref value1, ref value2, out result);
            return result;
        }
        #endregion

        /// <summary>
        /// Divides a vector by a factor.
        /// </summary>
        /// <param name="value1">The vector to divide.</param>
        /// <param name="scaleFactor">The scale factor.</param>
        /// <returns>Returns the scaled vector.</returns>
#if INLINE_ENABLE
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static TSVector3 operator /(TSVector3 value1, FP value2)
        {
            TSVector3 result;
            TSVector3.Divide(ref value1, value2, out result);
            return result;
        }

#if INLINE_ENABLE
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static TSVector3 operator /(TSVector3 value1, int value2)
        {
            TSVector3 result;
            TSVector3.Divide(ref value1, value2, out result);
            return result;
        }

#if INLINE_ENABLE
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static TSVector3 operator /(TSVector3 value1, long value2)
        {
            TSVector3 result;
            TSVector3.Divide(ref value1, value2, out result);
            return result;
        }

#if INLINE_ENABLE
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public TSVector2 ToTSVector2()
        {
            return new TSVector2(this.x, this.y);
        }

#if INLINE_ENABLE
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public TSVector4 ToTSVector4()
        {
            return new TSVector4(this.x, this.y, this.z, FP.One);
        }

    }

}