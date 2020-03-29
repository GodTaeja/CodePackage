using System;

namespace TrueSync
{
    [Serializable]
    public struct TSRect
    {
        private FP mXMin;

        private FP mYMin;

        private FP mWidth;

        private FP mHeight;

        public FP x
        {
            get
            {
                return this.mXMin;
            }
            set
            {
                this.mXMin = value;
            }
        }

        public FP y
        {
            get
            {
                return this.mYMin;
            }
            set
            {
                this.mYMin = value;
            }
        }

        public TSVector2 position
        {
            get
            {
                return new TSVector2(this.mXMin, this.mYMin);
            }
            set
            {
                this.mXMin = value.x;
                this.mYMin = value.y;
            }
        }

        public TSVector2 center
        {
            get
            {
                return new TSVector2(this.x + (this.mWidth  / 2 ), this.y + (this.mHeight / 2));
            }
            set
            {
                this.mXMin = value.x - (this.mWidth / 2);
                this.mYMin = value.y - (this.mHeight / 2);
            }
        }

        public TSVector2 min
        {
            get
            {
                return new TSVector2(this.xMin, this.yMin);
            }
            set
            {
                this.xMin = value.x;
                this.yMin = value.y;
            }
        }

        public TSVector2 max
        {
            get
            {
                return new TSVector2(this.xMax, this.yMax);
            }
            set
            {
                this.xMax = value.x;
                this.yMax = value.y;
            }
        }

        public FP width
        {
            get
            {
                return this.mWidth;
            }
            set
            {
                this.mWidth = value;
            }
        }

        public FP height
        {
            get
            {
                return this.mHeight;
            }
            set
            {
                this.mHeight = value;
            }
        }

        public TSVector2 size
        {
            get
            {
                return new TSVector2(this.mWidth, this.mHeight);
            }
            set
            {
                this.mWidth = value.x;
                this.mHeight = value.y;
            }
        }

        public FP xMin
        {
            get
            {
                return this.mXMin;
            }
            set
            {
                FP xMax = this.xMax;
                this.mXMin = value;
                this.mWidth = xMax - this.mXMin;
            }
        }

        public FP yMin
        {
            get
            {
                return this.mYMin;
            }
            set
            {
                FP yMax = this.yMax;
                this.mYMin = value;
                this.mHeight = yMax - this.mYMin;
            }
        }

        public FP xMax
        {
            get
            {
                return this.mWidth + this.mXMin;
            }
            set
            {
                this.mWidth = value - this.mXMin;
            }
        }

        public FP yMax
        {
            get
            {
                return this.mHeight + this.mYMin;
            }
            set
            {
                this.mHeight = value - this.mYMin;
            }
        }

        public TSRect(FP left, FP top, FP width, FP height)
        {
            this.mXMin = left;
            this.mYMin = top;
            this.mWidth = width;
            this.mHeight = height;
        }

        public TSRect(TSRect source)
        {
            this.mXMin = source.mXMin;
            this.mYMin = source.mYMin;
            this.mWidth = source.mWidth;
            this.mHeight = source.mHeight;
        }

        public static TSRect MinMaxRect(FP left, FP top, FP right, FP bottom)
        {
            return new TSRect(left, top, right - left, bottom - top);
        }

        public void Set(FP left, FP top, FP width, FP height)
        {
            this.mXMin = left;
            this.mYMin = top;
            this.mWidth = width;
            this.mHeight = height;
        }

        public override string ToString()
        {
            object[] array = new object[]
            {
            this.x,
            this.y,
            this.width,
            this.height
            };
            return string.Format("(x:{0:F2}, y:{1:F2}, width:{2:F2}, height:{3:F2})", array);
        }

        public string ToString(string format)
        {
            object[] array = new object[]
            {
            this.x.ToString(format),
            this.y.ToString(format),
            this.width.ToString(format),
            this.height.ToString(format)
            };
            return string.Format("(x:{0}, y:{1}, width:{2}, height:{3})", array);
        }

        public bool Contains(TSVector2 point)
        {
            return point.x >= this.xMin && point.x < this.xMax && point.y >= this.yMin && point.y < this.yMax;
        }

        public bool Contains(TSVector3 point)
        {
            return point.x >= this.xMin && point.x < this.xMax && point.y >= this.yMin && point.y < this.yMax;
        }

        public bool Contains(TSVector3 point, bool allowInverse)
        {
            if (!allowInverse)
            {
                return this.Contains(point);
            }
            bool flag = false;
            if (((float)this.width < 0f && point.x <= this.xMin && point.x > this.xMax) || ((float)this.width >= 0f && point.x >= this.xMin && point.x < this.xMax))
            {
                flag = true;
            }
            return flag && (((float)this.height < 0f && point.y <= this.yMin && point.y > this.yMax) || ((float)this.height >= 0f && point.y >= this.yMin && point.y < this.yMax));
        }

        private static TSRect OrderMinMax(TSRect rect)
        {
            if (rect.xMin > rect.xMax)
            {
                FP xMin = rect.xMin;
                rect.xMin = rect.xMax;
                rect.xMax = xMin;
            }
            if (rect.yMin > rect.yMax)
            {
                FP yMin = rect.yMin;
                rect.yMin = rect.yMax;
                rect.yMax = yMin;
            }
            return rect;
        }

        public bool Overlaps(TSRect other)
        {
            return other.xMax > this.xMin && other.xMin < this.xMax && other.yMax > this.yMin && other.yMin < this.yMax;
        }

        public bool Overlaps(TSRect other, bool allowInverse)
        {
            TSRect rect = this;
            if (allowInverse)
            {
                rect = TSRect.OrderMinMax(rect);
                other = TSRect.OrderMinMax(other);
            }
            return rect.Overlaps(other);
        }

        public override int GetHashCode()
        {
            return this.x.GetHashCode() ^ this.width.GetHashCode() << 2 ^ this.y.GetHashCode() >> 2 ^ this.height.GetHashCode() >> 1;
        }

        public override bool Equals(object other)
        {
            if (!(other is TSRect))
            {
                return false;
            }
            TSRect vRect = (TSRect)other;
            return this.x.Equals(vRect.x) && this.y.Equals(vRect.y) && this.width.Equals(vRect.width) && this.height.Equals(vRect.height);
        }

        public static bool operator !=(TSRect lhs, TSRect rhs)
        {
            return lhs.x != rhs.x || lhs.y != rhs.y || lhs.width != rhs.width || lhs.height != rhs.height;
        }

        public static bool operator ==(TSRect lhs, TSRect rhs)
        {
            return lhs.x == rhs.x && lhs.y == rhs.y && lhs.width == rhs.width && lhs.height == rhs.height;
        }
    }
}
