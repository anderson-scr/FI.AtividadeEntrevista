using FI.WebAtividadeEntrevista.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace FI.WebAtividadeEntrevista.CustomAttributes
{
    internal class Validacoes : ValidationAttribute
    {
        private readonly string _errorMsg;

        private readonly string _propertyToConcat;

        private readonly ETipoValidacao _tipoValidacao;

        private string CustomErrorMsg { get; set; }

        public Validacoes(ETipoValidacao tipoValidacao)
        {
            _tipoValidacao = tipoValidacao;
        }

        public Validacoes(ETipoValidacao tipoValidacao, string errorMsg)
        {
            _tipoValidacao = tipoValidacao;
            _errorMsg = errorMsg;
        }

        public Validacoes(ETipoValidacao tipoValidacao, string errorMsg, string propertyToConcat)
        {
            _tipoValidacao = tipoValidacao;
            _errorMsg = errorMsg;
            _propertyToConcat = propertyToConcat;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            try
            {
                string valor = value as string;

                if (valor != null)
                {
                    // Adicionar novos cases para novos tipos de validacoes
                    switch (_tipoValidacao)
                    {
                        case ETipoValidacao.Cpf:
                            VerificaNomeProperty(validationContext);
                            RemoverMascaraCpf(ref valor);
                            ValidaEstruturaCpf(valor);
                            ValidaCalculoCpfVeridico(valor);
                            AtualizarValor(valor, validationContext);
                            return ValidationResult.Success;

                        default:
                            return ValidationResult.Success;
                    }
                }
                else
                {
                    return ValidationResult.Success;
                }

            }
            catch (Exception ex)
            {
                if(String.IsNullOrEmpty(CustomErrorMsg))
                    return new ValidationResult(ex.Message);

                else
                    return new ValidationResult(CustomErrorMsg);
            }
        }

        private void VerificaNomeProperty(ValidationContext validationContext)
        {
            if(_propertyToConcat != null)
            {
                var nomeProperty = validationContext.ObjectType.GetProperty(_propertyToConcat);

                if (nomeProperty != null)
                {
                    var value = nomeProperty.GetValue(validationContext.ObjectInstance)?.ToString();
                    if (!string.IsNullOrEmpty(value))
                    {
                        // Concatenate 'Nome' value to the error message
                        CustomErrorMsg = String.Format(_errorMsg, value);
                    }
                }
            }
        }

        private void AtualizarValor(string cpf, ValidationContext validationContext)
        {
            var property = validationContext.ObjectType.GetProperty(validationContext.MemberName);

            if (property != null)
            {
                property.SetValue(validationContext.ObjectInstance, cpf, null);

            }
        }

        private void RemoverMascaraCpf(ref string cpf)
        {
            if (cpf.Contains(".") || cpf.Contains("-"))
            {
                cpf = cpf.Replace(".", "").Replace("-", "").Trim();
            }
        }

        private void ValidaEstruturaCpf(string cpf)
        {
            if (cpf.Count() != 11)
                throw new Exception("Estrutura de CPF inválido.");
        }

        private void ValidaCalculoCpfVeridico(string cpf)
        {
            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            for (int j = 0; j < 10; j++)
            {
                if (j.ToString().PadLeft(11, char.Parse(j.ToString())) == cpf)
                    throw new Exception("O CPF informado é inválido");
            }

            string tempCpf = cpf.Substring(0, 9);
            int soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

            int resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            string digito = resto.ToString();
            tempCpf = tempCpf + digito;
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = digito + resto.ToString();

            if (!cpf.EndsWith(digito))
                throw new Exception("O CPF informado é inválido");
        }
    }
}
