

namespace approx_sort
{
    public partial class Form1 : Form
    {

        List<int> list = new();

        int width
        {
            get 
            {
                return Math.Max(1,(Size.Width - 200) / list.Count); 
            }
        }

        const int n = 1000;

        int upper = 5000;
        int lower = 0;
        int lows = 0;
        int highs = n-1;
        int lastlowest = 0;
        int lasthighest = 500;

        int lowi = 0;
        int highi = n - 1;

        int t = 0;

        Random rnd = new Random();

        public Form1()
        {
            InitializeComponent();
            Size = new Size(Math.Max(n+200, 1000), upper +200);
            Paint += paint;
            MouseDown += (object o, MouseEventArgs mea) => { shake(); t++; this.Invalidate(); };
            KeyPress += Key;
            this.DoubleBuffered = true;

            form(n);
            pass();
            //approx();

            this.Invalidate();
        }

        void Key(object o, KeyPressEventArgs kea) 
        {
            t++;
            if (kea.KeyChar == ' ')
            {
                pass();
                approx();
                mergeSize = 2;
            }
            if (kea.KeyChar == 'm')
            {
                merge();
            }
            if (kea.KeyChar == 'f')
            {
                fix();
                mergeSize = 2;
            }
            if (kea.KeyChar == 'r')
            {
                mergeSize = 2;
                fixn = 1;
                form(n);
                lows = 0;
                highs = n-1;
                lasthighest = 500;
                lastlowest = 0;
                t = 0;
            }
            
            
            this.Invalidate();
        }

        int fixn = 1;

        void fix ()
        {

            int l = lows;
            int r = highs;

            int fixSize = n / fixn;

            for (int i = 0; i < fixn; i++)
            {
                l = i * fixSize;
                r = Math.Min((i+1) * fixSize - 1, n-1);

                int midindex = (l + r) / 2;
                int mid = 0;
                while (inv(mid) < midindex)
                {
                    mid++;
                }

                while (l < r)
                {
                    if (list[l] > mid)
                    {
                        if (list[r] < mid)
                        {
                            s(l, r);
                            l++;
                            r--;
                        }
                        else
                        {
                            r--;
                        }
                    }
                    else
                    {
                        if (list[r] < mid)
                        {
                            l++;
                        }
                        else
                        {
                            r--;
                            l++;
                        }
                    }

                }

            }
            fixn *= 2;


        }

        int mergeSize = 2;


        void merge ()
        {
            int j = 0;
            List<int> ints = new List<int>();
            int l = 0;
            int r = 0;
            int leftEnd = 0;
            int rightEnd = 0;
            for (int i = 0; i < list.Count; i++)
            {

                if (i % mergeSize == 0)
                {
                    ints = new();
                    l = i;
                    r = i + mergeSize/2;
                    leftEnd = Math.Min(r, list.Count);
                    rightEnd = Math.Min(i+mergeSize, list.Count);
                }
                if (r >= rightEnd)
                {
                    ints.Add(list[l]);
                    l++;
                }
                else
                if (l >= leftEnd)
                {
                    ints.Add(list[r]);
                    r++;
                }
                else
                if (list[l] < list[r])
                {
                    ints.Add(list[l]);
                    l++;
                }
                else
                {
                    ints.Add(list[r]);
                    r++;
                }

                if (i % mergeSize == mergeSize - 1 || i == list.Count - 1)
                {
                    for (int k = 0; k < mergeSize && j < list.Count; k++)
                    {
                        list[j] = ints[k];
                        j++;
                    }
                }
            }
            mergeSize *= 2;
        }

        void form(int l)
        {
            list.Clear();
            for (int i = 0; i < l; i++)
            {
                list.Add(rnd.Next() % (upper - 11) + 10);
            }
        }

        void paint (object o, PaintEventArgs pea){

            pea.Graphics.DrawString($"Steps: {t}", Font, new SolidBrush(Color.Green), new Point(0, 0));

            float hh = 500f / upper;

            for (int i = 0; i < lows; i++)
            {
                pea.Graphics.FillRectangle(new SolidBrush(Color.Red), new(i * width + 100, 600 - (int)(hh *list[i]), width, (int)(hh*list[i])));
            }
            for (int i = lows; i < highs+1; i++)
            {
                pea.Graphics.FillRectangle(new SolidBrush(Color.Green), new(i * width + 100, 600 - (int)(hh * list[i]), width, (int)(hh * list[i])));
            }
            for (int i = highs+1; i < list.Count; i++)
            {
                pea.Graphics.FillRectangle(new SolidBrush(Color.Blue), new(i * width + 100, 600 - (int)(hh * list[i]), width, (int)(hh * list[i])));
            }

            //pea.Graphics.FillRectangle(new SolidBrush(Color.Orange), new(lowi * width + 100, 600 - list[lowi], width, list[lowi]));
            //pea.Graphics.FillRectangle(new SolidBrush(Color.LightBlue), new(highi * width + 100, 600 - list[highi], width, list[highi]));


        }

        int inv(int n)
        {
            int inv = (highs - lows) * (n - lastlowest) / (lasthighest - lastlowest) + lows;

            return inv;
        }

        void approx ()
        {
            int lowest = upper;
            int highest = lower;

            for (int i = lows; i < highs+1; i++)
            {
                int n = list[i];
                int invers = inv(n);

                s(i, invers);
            }
            pass();
        }

        void shake()
        {
            int lastmerge = 0;

            for (int i = lows; i < highs; i++)
            {
                if (list[i] > list[i + 1])
                {
                    s(i, i + 1);
                    lastmerge = i+1;
                }

            }
            highs=lastmerge;

            for (int i = highs; i > lows; i--)
            {
                if (list[i] < list[i - 1])
                {
                    s(i, i - 1);
                    lastmerge = i-1;
                }

            }
            lows = lastmerge;

        }


        void pass ()
        {
            int lowest = upper;
            int highest = lower;

            

            for (int i = lows;i < highs+1;i++)
            {
                if (list[i] < lowest) { lowest = list[i]; lowi = i; }  ;
                if (list[i] > highest) { highest = list[i]; highi = i; } ;
            }

            if (highi == lows)
            {
                s(lowi, lows);
                s(lowi, highs);
            }
            else
            {
                s(lowi, lows);
                s(highi, highs);
            }

            lows++;
            highs--;

            lastlowest = lowest;
            lasthighest = highest;

        }

        void s(int i1, int i2)
        {
            int n = list[i1];
            list[i1] = list[i2];
            list[i2] = n;
        }
    }
}