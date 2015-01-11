using System;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Globalization;

namespace Fractals {

    [Serializable]
    public struct RectangleD {

        /// <include file='doc\RectangleD.uex' path='docs/doc[@for="RectangleD.Empty"]/*' />
        /// <devdoc>
        ///    Initializes a new instance of the <see cref='RectangleD'/>
        ///    class.
        /// </devdoc>
        public static readonly RectangleD Empty = new RectangleD();

        private double x;
        private double y;
        private double width;
        private double height;

        /// <include file='doc\RectangleD.uex' path='docs/doc[@for="RectangleD.RectangleD"]/*' />
        /// <devdoc>
        ///    <para>
        ///       Initializes a new instance of the <see cref='RectangleD'/>
        ///       class with the specified location and size.
        ///    </para>
        /// </devdoc>
        public RectangleD(double x, double y, double width, double height) {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        /// <include file='doc\RectangleD.uex' path='docs/doc[@for="RectangleD.RectangleD1"]/*' />
        /// <devdoc>
        ///    <para>
        ///       Initializes a new instance of the <see cref='System.Drawing.RectangleD'/>
        ///       class with the specified location
        ///       and size.
        ///    </para>
        /// </devdoc>
        public RectangleD(PointD location, SizeF size) {
            this.x = location.X;
            this.y = location.Y;
            this.width = size.Width;
            this.height = size.Height;
        }

        /// <include file='doc\RectangleD.uex' path='docs/doc[@for="RectangleD.FromLTRB"]/*' />
        /// <devdoc>
        ///    <para>
        ///       Creates a new <see cref='System.Drawing.RectangleD'/> with
        ///       the specified location and size.
        ///    </para>
        /// </devdoc>
        // !! Not in C++ version
        [System.Runtime.TargetedPatchingOptOutAttribute("Performance critical to inline across NGen image boundaries")]
        public static RectangleD FromLTRB(double left, double top, double right, double bottom) {
            return new RectangleD(left,
                top,
                right - left,
                bottom - top);
        }

        /// <include file='doc\RectangleD.uex' path='docs/doc[@for="RectangleD.Location"]/*' />
        /// <devdoc>
        ///    <para>
        ///       Gets or sets the coordinates of the upper-left corner of
        ///       the rectangular region represented by this <see cref='System.Drawing.RectangleD'/>.
        ///    </para>
        /// </devdoc>
        [Browsable(false)]
        public PointD Location {
            [System.Runtime.TargetedPatchingOptOutAttribute("Performance critical to inline across NGen image boundaries")] get { return new PointD(X, Y); }
            set {
                X = value.X;
                Y = value.Y;
            }
        }

        /// <include file='doc\RectangleD.uex' path='docs/doc[@for="RectangleD.Size"]/*' />
        /// <devdoc>
        ///    <para>
        ///       Gets or sets the size of this <see cref='RectangleD'/>.
        ///    </para>
        /// </devdoc>
        [Browsable(false)]
        public SizeF Size {
            [System.Runtime.TargetedPatchingOptOutAttribute("Performance critical to inline across NGen image boundaries")] get { return new SizeF(Width, Height); }
            set {
                this.Width = value.Width;
                this.Height = value.Height;
            }
        }

        /// <include file='doc\RectangleD.uex' path='docs/doc[@for="RectangleD.X"]/*' />
        /// <devdoc>
        ///    <para>
        ///       Gets or sets the x-coordinate of the
        ///       upper-left corner of the rectangular region defined by this <see cref='RectangleD'/>.
        ///    </para>
        /// </devdoc>
        public double X {
            get { return x; }
            set { x = value; }
        }

        /// <include file='doc\RectangleD.uex' path='docs/doc[@for="RectangleD.Y"]/*' />
        /// <devdoc>
        ///    <para>
        ///       Gets or sets the y-coordinate of the
        ///       upper-left corner of the rectangular region defined by this <see cref='System.Drawing.RectangleD'/>.
        ///    </para>
        /// </devdoc>
        public double Y {
            get { return y; }
            set { y = value; }
        }

        /// <include file='doc\RectangleD.uex' path='docs/doc[@for="RectangleD.Width"]/*' />
        /// <devdoc>
        ///    <para>
        ///       Gets or sets the width of the rectangular
        ///       region defined by this <see cref='System.Drawing.RectangleD'/>.
        ///    </para>
        /// </devdoc>
        public double Width {
            get { return width; }
            set { width = value; }
        }

        /// <include file='doc\RectangleD.uex' path='docs/doc[@for="RectangleD.Height"]/*' />
        /// <devdoc>
        ///    <para>
        ///       Gets or sets the height of the
        ///       rectangular region defined by this <see cref='System.Drawing.RectangleD'/>.
        ///    </para>
        /// </devdoc>
        public double Height {
            get { return height; }
            set { height = value; }
        }

        /// <include file='doc\RectangleD.uex' path='docs/doc[@for="RectangleD.Left"]/*' />
        /// <devdoc>
        ///    <para>
        ///       Gets the x-coordinate of the upper-left corner of the
        ///       rectangular region defined by this <see cref='System.Drawing.RectangleD'/> .
        ///    </para>
        /// </devdoc>
        [Browsable(false)]
        public double Left {
            get { return X; }
        }

        /// <include file='doc\RectangleD.uex' path='docs/doc[@for="RectangleD.Top"]/*' />
        /// <devdoc>
        ///    <para>
        ///       Gets the y-coordinate of the upper-left corner of the
        ///       rectangular region defined by this <see cref='System.Drawing.RectangleD'/>.
        ///    </para>
        /// </devdoc>
        [Browsable(false)]
        public double Top {
            get { return Y; }
        }

        /// <include file='doc\RectangleD.uex' path='docs/doc[@for="RectangleD.Right"]/*' />
        /// <devdoc>
        ///    <para>
        ///       Gets the x-coordinate of the lower-right corner of the
        ///       rectangular region defined by this <see cref='System.Drawing.RectangleD'/>.
        ///    </para>
        /// </devdoc>
        [Browsable(false)]
        public double Right {
            [System.Runtime.TargetedPatchingOptOutAttribute("Performance critical to inline across NGen image boundaries")] get { return X + Width; }
        }

        /// <include file='doc\RectangleD.uex' path='docs/doc[@for="RectangleD.Bottom"]/*' />
        /// <devdoc>
        ///    <para>
        ///       Gets the y-coordinate of the lower-right corner of the
        ///       rectangular region defined by this <see cref='System.Drawing.RectangleD'/>.
        ///    </para>
        /// </devdoc>
        [Browsable(false)]
        public double Bottom {
            [System.Runtime.TargetedPatchingOptOutAttribute("Performance critical to inline across NGen image boundaries")] get { return Y + Height; }
        }

        /// <include file='doc\RectangleD.uex' path='docs/doc[@for="RectangleD.IsEmpty"]/*' />
        /// <devdoc>
        ///    <para>
        ///       Tests whether this <see cref='System.Drawing.RectangleD'/> has a <see cref='System.Drawing.RectangleD.Width'/> or a <see cref='System.Drawing.RectangleD.Height'/> of 0.
        ///    </para>
        /// </devdoc>
        [Browsable(false)]
        public bool IsEmpty {
            get { return (Width <= 0) || (Height <= 0); }
        }

        /// <include file='doc\RectangleD.uex' path='docs/doc[@for="RectangleD.Equals"]/*' />
        /// <devdoc>
        ///    <para>
        ///       Tests whether <paramref name="obj"/> is a <see cref='System.Drawing.RectangleD'/> with the same location and size of this
        ///    <see cref='System.Drawing.RectangleD'/>.
        ///    </para>
        /// </devdoc>
        public override bool Equals(object obj) {
            if (!(obj is RectangleD))
                return false;

            RectangleD comp = (RectangleD) obj;

            return (comp.X == this.X) &&
                   (comp.Y == this.Y) &&
                   (comp.Width == this.Width) &&
                   (comp.Height == this.Height);
        }

        /// <include file='doc\RectangleD.uex' path='docs/doc[@for="RectangleD.operator=="]/*' />
        /// <devdoc>
        ///    <para>
        ///       Tests whether two <see cref='System.Drawing.RectangleD'/>
        ///       objects have equal location and size.
        ///    </para>
        /// </devdoc>
        public static bool operator ==(RectangleD left, RectangleD right) {
            return (left.X == right.X
                    && left.Y == right.Y
                    && left.Width == right.Width
                    && left.Height == right.Height);
        }

        /// <include file='doc\RectangleD.uex' path='docs/doc[@for="RectangleD.operator!="]/*' />
        /// <devdoc>
        ///    <para>
        ///       Tests whether two <see cref='System.Drawing.RectangleD'/>
        ///       objects differ in location or size.
        ///    </para>
        /// </devdoc>
        public static bool operator !=(RectangleD left, RectangleD right) {
            return !(left == right);
        }

        /// <include file='doc\RectangleD.uex' path='docs/doc[@for="RectangleD.Contains"]/*' />
        /// <devdoc>
        ///    <para>
        ///       Determines if the specfied point is contained within the
        ///       rectangular region defined by this <see cref='System.Drawing.Rectangle'/> .
        ///    </para>
        /// </devdoc>
        [Pure]
        public bool Contains(double x, double y) {
            return this.X <= x &&
                   x < this.X + this.Width &&
                   this.Y <= y &&
                   y < this.Y + this.Height;
        }

        /// <include file='doc\RectangleD.uex' path='docs/doc[@for="RectangleD.Contains1"]/*' />
        /// <devdoc>
        ///    <para>
        ///       Determines if the specfied point is contained within the
        ///       rectangular region defined by this <see cref='System.Drawing.Rectangle'/> .
        ///    </para>
        /// </devdoc>
        [Pure]
        public bool Contains(PointD pt) {
            return Contains(pt.X, pt.Y);
        }

        /// <include file='doc\RectangleD.uex' path='docs/doc[@for="RectangleD.Contains2"]/*' />
        /// <devdoc>
        ///    <para>
        ///       Determines if the rectangular region represented by
        ///    <paramref name="rect"/> is entirely contained within the rectangular region represented by 
        ///       this <see cref='System.Drawing.Rectangle'/> .
        ///    </para>
        /// </devdoc>
        [Pure]
        public bool Contains(RectangleD rect) {
            return (this.X <= rect.X) &&
                   ((rect.X + rect.Width) <= (this.X + this.Width)) &&
                   (this.Y <= rect.Y) &&
                   ((rect.Y + rect.Height) <= (this.Y + this.Height));
        }

        // !! Not in C++ version
        /// <include file='doc\RectangleD.uex' path='docs/doc[@for="RectangleD.GetHashCode"]/*' />
        /// <devdoc>
        ///    Gets the hash code for this <see cref='System.Drawing.RectangleD'/>.
        /// </devdoc>
        public override int GetHashCode() {
            return unchecked((int) ((UInt32) X ^
                                    (((UInt32) Y << 13) | ((UInt32) Y >> 19)) ^
                                    (((UInt32) Width << 26) | ((UInt32) Width >> 6)) ^
                                    (((UInt32) Height << 7) | ((UInt32) Height >> 25))));
        }

        /// <include file='doc\RectangleD.uex' path='docs/doc[@for="RectangleD.Inflate"]/*' />
        /// <devdoc>
        ///    <para>
        ///       Inflates this <see cref='System.Drawing.Rectangle'/>
        ///       by the specified amount.
        ///    </para>
        /// </devdoc>
        public void Inflate(double x, double y) {
            this.X -= x;
            this.Y -= y;
            this.Width += 2 * x;
            this.Height += 2 * y;
        }

        /// <include file='doc\RectangleD.uex' path='docs/doc[@for="RectangleD.Inflate1"]/*' />
        /// <devdoc>
        ///    Inflates this <see cref='System.Drawing.Rectangle'/> by the specified amount.
        /// </devdoc>
        public void Inflate(SizeF size) {
            Inflate(size.Width, size.Height);
        }

        /// <include file='doc\RectangleD.uex' path='docs/doc[@for="RectangleD.Inflate2"]/*' />
        /// <devdoc>
        ///    <para>
        ///       Creates a <see cref='System.Drawing.Rectangle'/>
        ///       that is inflated by the specified amount.
        ///    </para>
        /// </devdoc>
        // !! Not in C++
        public static RectangleD Inflate(RectangleD rect, double x, double y) {
            RectangleD r = rect;
            r.Inflate(x, y);
            return r;
        }

        /// <include file='doc\RectangleD.uex' path='docs/doc[@for="RectangleD.Intersect"]/*' />
        /// <devdoc> Creates a Rectangle that represents the intersection between this Rectangle and rect.
        /// </devdoc>
        public void Intersect(RectangleD rect) {
            RectangleD result = RectangleD.Intersect(rect, this);

            this.X = result.X;
            this.Y = result.Y;
            this.Width = result.Width;
            this.Height = result.Height;
        }

        /// <include file='doc\RectangleD.uex' path='docs/doc[@for="RectangleD.Intersect1"]/*' />
        /// <devdoc>
        ///    Creates a rectangle that represents the intersetion between a and
        ///    b. If there is no intersection, null is returned.
        /// </devdoc>
        [Pure]
        public static RectangleD Intersect(RectangleD a, RectangleD b) {
            double x1 = Math.Max(a.X, b.X);
            double x2 = Math.Min(a.X + a.Width, b.X + b.Width);
            double y1 = Math.Max(a.Y, b.Y);
            double y2 = Math.Min(a.Y + a.Height, b.Y + b.Height);

            if (x2 >= x1
                && y2 >= y1) {

                return new RectangleD(x1, y1, x2 - x1, y2 - y1);
            }
            return RectangleD.Empty;
        }

        /// <include file='doc\RectangleD.uex' path='docs/doc[@for="RectangleD.IntersectsWith"]/*' />
        /// <devdoc>
        ///    Determines if this rectangle intersets with rect.
        /// </devdoc>
        [Pure]
        public bool IntersectsWith(RectangleD rect) {
            return (rect.X < this.X + this.Width) &&
                   (this.X < (rect.X + rect.Width)) &&
                   (rect.Y < this.Y + this.Height) &&
                   (this.Y < rect.Y + rect.Height);
        }

        /// <include file='doc\RectangleD.uex' path='docs/doc[@for="RectangleD.Union"]/*' />
        /// <devdoc>
        ///    Creates a rectangle that represents the union between a and
        ///    b.
        /// </devdoc>
        [Pure]
        public static RectangleD Union(RectangleD a, RectangleD b) {
            double x1 = Math.Min(a.X, b.X);
            double x2 = Math.Max(a.X + a.Width, b.X + b.Width);
            double y1 = Math.Min(a.Y, b.Y);
            double y2 = Math.Max(a.Y + a.Height, b.Y + b.Height);

            return new RectangleD(x1, y1, x2 - x1, y2 - y1);
        }

        /// <include file='doc\RectangleD.uex' path='docs/doc[@for="RectangleD.Offset"]/*' />
        /// <devdoc>
        ///    Adjusts the location of this rectangle by the specified amount.
        /// </devdoc>
        public void Offset(PointD pos) {
            Offset(pos.X, pos.Y);
        }

        /// <include file='doc\RectangleD.uex' path='docs/doc[@for="RectangleD.Offset1"]/*' />
        /// <devdoc>
        ///    Adjusts the location of this rectangle by the specified amount.
        /// </devdoc>
        public void Offset(double x, double y) {
            this.X += x;
            this.Y += y;
        }

        /// <include file='doc\RectangleD.uex' path='docs/doc[@for="RectangleD.operatorRectangleD"]/*' />
        /// <devdoc>
        ///    Converts the specified <see cref='System.Drawing.Rectangle'/> to a
        /// <see cref='System.Drawing.RectangleD'/>.
        /// </devdoc>
        public static implicit operator RectangleD(Rectangle r) {
            return new RectangleD(r.X, r.Y, r.Width, r.Height);
        }

        /// <include file='doc\RectangleD.uex' path='docs/doc[@for="RectangleD.ToString"]/*' />
        /// <devdoc>
        ///    Converts the <see cref='System.Drawing.RectangleD.Location'/> and <see cref='System.Drawing.RectangleD.Size'/> of this <see cref='System.Drawing.RectangleD'/> to a
        ///    human-readable string.
        /// </devdoc>
        public override string ToString() {
            return "{X=" + X.ToString(CultureInfo.CurrentCulture) + ",Y=" + Y.ToString(CultureInfo.CurrentCulture) +
                   ",Width=" + Width.ToString(CultureInfo.CurrentCulture) +
                   ",Height=" + Height.ToString(CultureInfo.CurrentCulture) + "}";
        }
    }
}