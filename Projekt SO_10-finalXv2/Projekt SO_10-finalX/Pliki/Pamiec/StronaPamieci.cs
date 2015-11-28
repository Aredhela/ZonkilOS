    class StronaPamieci
    {
        public const int Rozmiar = 16;

        /// <summary>
        /// Adres w pamieci fizycznej w ktorym zaczyna sie dana ramka
        /// Adresy ujemne oznaczaja, ze ramka zostala 
        /// </summary>
        public int AdresPoczatkowy;
        public bool JestZmapowanaWPamieciFizycznej;
        public bool JestNaDysku;

        public override string ToString()
        {
            return string.Format("Adres: {0,8} Zmapowany: {1} Na dysku: {2}", AdresPoczatkowy, JestZmapowanaWPamieciFizycznej, JestNaDysku);
        }
    }
