using System;

namespace Atendimento.Infra.Base
{
    /// <summary>
    /// Classe que define um custom attribute
    /// </summary>
    public class StringValue : Attribute
    {
        private string _value;

        public StringValue(string value)
        {
            _value = value;
        }

        public string Value
        {
            get { return _value; }
        }
    }
}
