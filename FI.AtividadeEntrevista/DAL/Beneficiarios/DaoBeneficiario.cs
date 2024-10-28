using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace FI.AtividadeEntrevista.DAL
{
    internal class DaoBeneficiario : AcessoDados
    {
        internal long Incluir(DML.Beneficiario beneficiario)
        {
            List<SqlParameter> parametros = new List<SqlParameter>();

            parametros.Add(new SqlParameter("Nome", beneficiario.Nome));
            parametros.Add(new SqlParameter("Cpf", beneficiario.CPF));
            parametros.Add(new SqlParameter("IdCliente", beneficiario.IdCliente));

            DataSet ds = Consultar("FI_SP_IncBenef", parametros);

            long ret = 0;
            if (ds.Tables[0].Rows.Count > 0)
                long.TryParse(ds.Tables[0].Rows[0][0].ToString(), out ret);
            return ret;
        }

        internal List<DML.Beneficiario> Consultar(long idCliente)
        {
            List<SqlParameter> parametros = new List<SqlParameter>();

            parametros.Add(new SqlParameter("IdCliente", idCliente));

            DataSet ds = base.Consultar("FI_SP_ConsBenef", parametros);
            List<DML.Beneficiario> cli = Converter(ds);

            return cli;
        }

        internal void Alterar(DML.Beneficiario beneficiario)
        {
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(new SqlParameter("Nome", beneficiario.Nome));
            parametros.Add(new SqlParameter("CPF", beneficiario.CPF));
            parametros.Add(new SqlParameter("ID", beneficiario.Id));

            base.Executar("FI_SP_AltBenef", parametros);
        }

        internal void Excluir(long IdCliente)
        {
            List<SqlParameter> parametros = new List<SqlParameter>();

            parametros.Add(new SqlParameter("IdCliente", IdCliente));

            base.Executar("FI_SP_DelBenef", parametros);
        }
        
        internal bool VerificarExistencia(string CPF)
        {
            List<SqlParameter> parametros = new List<SqlParameter>();

            parametros.Add(new SqlParameter("CPF", CPF));

            DataSet ds = base.Consultar("FI_SP_VerificaBenef", parametros);

            return ds.Tables[0].Rows.Count > 0;
        }

        internal List<DML.Beneficiario> Listar()
        {
            List<SqlParameter> parametros = new List<SqlParameter>();

            parametros.Add(new SqlParameter("Id", 0));

            DataSet ds = base.Consultar("FI_SP_ConsBenef", parametros);
            List<DML.Beneficiario> cli = Converter(ds);

            return cli;
        }

        private List<DML.Beneficiario> Converter(DataSet ds)
        {
            List<DML.Beneficiario> lista = new List<DML.Beneficiario>();

            if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    DML.Beneficiario beneficiario = new DML.Beneficiario();
                    beneficiario.Id = row.Field<long>("Id");
                    beneficiario.CPF = row.Field<string>("CPF");
                    beneficiario.Nome = row.Field<string>("Nome");
                    beneficiario.IdCliente = row.Field<long>("IdCliente");
                    
                    lista.Add(beneficiario);
                }
            }

            return lista;
        }
    }
}
