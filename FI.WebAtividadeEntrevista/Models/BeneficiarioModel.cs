using FI.WebAtividadeEntrevista.CustomAttributes;
using FI.WebAtividadeEntrevista.Enums;
using System.ComponentModel.DataAnnotations;

namespace WebAtividadeEntrevista.Models
{
    /// <summary>
    /// Classe de Modelo de Beneficiarios
    /// </summary>
    public class BeneficiarioModel
    {
        /// <summary>
        /// ID
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// CPF
        /// </summary>
        [Required]
        [MaxLength(14)]
        [Validacoes(ETipoValidacao.Cpf, "O CPF do beneficiário <b>{0}</b> informado é inválido.", "Nome")]
        public string CPF { get; set; }

        /// <summary>
        /// Nome
        /// </summary>
        [Required]
        public string Nome { get; set; }

        /// <summary>
        /// IdCliente
        /// </summary>
        [Required]
        public long IdCliente { get; set; }
    }
}
