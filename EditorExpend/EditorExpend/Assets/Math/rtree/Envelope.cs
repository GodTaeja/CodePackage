using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueSync;


namespace Enyim.Collections
{
	public class Envelope
	{
		internal Envelope() { }

		public Envelope(FP x1, FP y1, FP x2, FP y2)
		{
			X1 = x1;
			Y1 = y1;
			X2 = x2;
			Y2 = y2;
		}

		public FP X1 { get; private set; } // 0
		public FP Y1 { get; private set; } // 1
		public FP X2 { get; private set; } // 2
		public FP Y2 { get; private set; } // 3

		internal FP Area { get { return (X2 - X1) * (Y2 - Y1); } }
		internal FP Margin { get { return (X2 - X1) + (Y2 - Y1); } }

		internal void Extend(Envelope by)
		{
			X1 = TSMath.Min(X1, by.X1);
			Y1 = TSMath.Min(Y1, by.Y1);
			X2 = TSMath.Max(X2, by.X2);
			Y2 = TSMath.Max(Y2, by.Y2);
		}

		public override string ToString()
		{
			return String.Format("{0},{1} - {2},{3}", X1, Y1, X2, Y2);
		}

		internal bool Intersects(Envelope b)
        {
            //if (b == null)
            //    return false;
			return b.X1 <= X2 && b.Y1 <= Y2 && b.X2 >= X1 && b.Y2 >= Y1;
		}

		internal bool Contains(Envelope b)
		{
			return X1 <= b.X1 && Y1 <= b.Y1 && b.X2 <= X2 && b.Y2 <= Y2;
		}

        public FP Distance(FP x,FP y)
        {
            FP distanceSquared = 0;
            FP greatestMin = TSMath.Max(X1, x);
            FP leastMax = TSMath.Min(X2, x);
            if (greatestMin > leastMax)
            {
                distanceSquared += TSMath.Pow(greatestMin - leastMax,2);
            }
            greatestMin = TSMath.Max(Y1, y);
            leastMax = TSMath.Min(Y2, y);
            if (greatestMin > leastMax)
            {
                distanceSquared += TSMath.Pow(greatestMin - leastMax,2);
            }

            return (FP)TSMath.Sqrt(distanceSquared);
        }
    }
}

#region [ License information          ]

/* ************************************************************
 *
 *    Copyright (c) Attila Kiskó, enyim.com
 *
 *    Licensed under the Apache License, Version 2.0 (the "License");
 *    you may not use this file except in compliance with the License.
 *    You may obtain a copy of the License at
 *
 *        http://www.apache.org/licenses/LICENSE-2.0
 *
 *    Unless required by applicable law or agreed to in writing, software
 *    distributed under the License is distributed on an "AS IS" BASIS,
 *    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *    See the License for the specific language governing permissions and
 *    limitations under the License.
 *
 * ************************************************************/

#endregion
