 class BlokPamieci
    {
        public int StronaPoczatkowa;
        public int Blokow;
        public BlokPamieci Nastepny;

        public BlokPamieci(int stronaPoczatkowa, int blokow)
        {
            StronaPoczatkowa = stronaPoczatkowa;
            Blokow = blokow;
        }

        public override string ToString()
        {
            int AdresPoczatkowy = StronaPoczatkowa*Pamiec.RozmiarBloku;
            int Dlugosc = Blokow*Pamiec.RozmiarBloku;
            int AdresKoncowy = AdresPoczatkowy + Dlugosc;
            return string.Format("Od: {0,8}\t Do: {1,8}\t Dlugosc: {2,5} bajtow", AdresPoczatkowy, AdresKoncowy, Dlugosc);
        }
    }
