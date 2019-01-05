﻿namespace Atendimento.Entities.Entities
{
    public class Classificacao : BaseEntity
    {
        private string _nome;

        public string Nome
        {
            get
            {
                return _nome;
            }
            set
            {
                _nome = value.ToString().ToUpper();
            }
        }
    }
}