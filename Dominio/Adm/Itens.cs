    public class Itens
    {

        private int _quantidade;
        public int Quantidade
        {
            get
            {
                return _quantidade;
            }
            set
            {
                _quantidade = value;
            }
        }

        private int _codigo;
        public int Codigo
        {
            get
            {
                return _codigo;
            }
            set
            {
                _codigo = value;
            }
        }

        private bool _isNull;
        public bool ISNull
        {
            get
            {
                return _isNull;
            }
            set
            {
                _isNull = value;
            }
        }

        public Itens(int p_Codigo, int p_Quantidade)
        {
            this.Codigo = p_Codigo;
            this.Quantidade = p_Quantidade;
        }

    }
