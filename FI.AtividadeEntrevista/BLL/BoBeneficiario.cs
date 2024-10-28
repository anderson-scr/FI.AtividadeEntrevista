using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FI.AtividadeEntrevista.BLL
{
    public class BoBeneficiario
    {
        public long Incluir(DML.Beneficiario beneficiario)
        {
            DAL.DaoBeneficiario benef = new DAL.DaoBeneficiario();
            return benef.Incluir(beneficiario);
        }

        public void Alterar(DML.Beneficiario beneficiario)
        {
            DAL.DaoBeneficiario benef = new DAL.DaoBeneficiario();
            benef.Alterar(beneficiario);
        }

        public List<DML.Beneficiario> Consultar(long idCliente)
        {
            DAL.DaoBeneficiario benef = new DAL.DaoBeneficiario();
            return benef.Consultar(idCliente);
        }

        public void Excluir(long IdCliente)
        {
            DAL.DaoBeneficiario benef = new DAL.DaoBeneficiario();
            benef.Excluir(IdCliente);
        }

        public List<DML.Beneficiario> Listar()
        {
            DAL.DaoBeneficiario benef = new DAL.DaoBeneficiario();
            return benef.Listar();
        }

        public bool VerificarExistencia(string CPF)
        {
            DAL.DaoBeneficiario benef = new DAL.DaoBeneficiario();
            return benef.VerificarExistencia(CPF);
        }
    }
}
