using FI.AtividadeEntrevista.BLL;
using WebAtividadeEntrevista.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FI.AtividadeEntrevista.DML;

namespace WebAtividadeEntrevista.Controllers
{
    public class ClienteController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Incluir()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Incluir(ClienteModel model)
        {
            
            if (!this.ModelState.IsValid)
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

                Response.StatusCode = 400;
                return Json(string.Join("<br>", erros));
            }
            else
            {
                BoCliente bo = new BoCliente();

                bool cpfExistente = bo.VerificarExistencia(model.CPF);
                if (cpfExistente)
                {
                    Response.StatusCode = 400;
                    return Json("O CPF informado ja esta cadastro.");
                }


                model.Id = bo.Incluir(new Cliente()
                {
                    CEP = model.CEP,
                    Cidade = model.Cidade,
                    Email = model.Email,
                    Estado = model.Estado,
                    Logradouro = model.Logradouro,
                    Nacionalidade = model.Nacionalidade,
                    Nome = model.Nome,
                    Sobrenome = model.Sobrenome,
                    Telefone = model.Telefone,
                    CPF = model.CPF,
                });


                // Adiciona os beneficiarios se tiver
                if(model?.Beneficiarios != null)
                {
                    BoBeneficiario boBenef = new BoBeneficiario();
                    foreach(BeneficiarioModel modelBenef in model.Beneficiarios)
                    {
                        Beneficiario benef = new Beneficiario()
                        {
                            CPF = modelBenef.CPF,
                            Nome = modelBenef.Nome,
                            IdCliente = model.Id
                        };
                        boBenef.Incluir(benef);
                    }
                }

                return Json("Cadastro efetuado com sucesso");
            }
        }

        [HttpPost]
        public JsonResult Alterar(ClienteModel model)
        {
       
            if (!this.ModelState.IsValid)
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

                Response.StatusCode = 400;
                return Json(string.Join("<br>", erros));
            }
            else
            {
                BoCliente bo = new BoCliente();

                bool cpfExistente = bo.VerificarExistencia(model.CPF, model.Id);
                if (cpfExistente)
                {
                    Response.StatusCode = 400;
                    return Json("O CPF informado ja esta cadastro.");
                }

                bo.Alterar(new Cliente()
                {
                    Id = model.Id,
                    CEP = model.CEP,
                    Cidade = model.Cidade,
                    Email = model.Email,
                    Estado = model.Estado,
                    Logradouro = model.Logradouro,
                    Nacionalidade = model.Nacionalidade,
                    Nome = model.Nome,
                    Sobrenome = model.Sobrenome,
                    Telefone = model.Telefone,
                    CPF = model.CPF
                });


                // Adiciona os beneficiarios se tiver
                // Por falta de tempo, vou excluir tudo e salvar novamente. Esse fluxo nn e correto.

                BoBeneficiario boBenef = new BoBeneficiario();
                boBenef.Excluir(model.Id);

                if (model?.Beneficiarios != null)
                {
                    foreach (BeneficiarioModel modelBenef in model.Beneficiarios)
                    {
                        Beneficiario benef = new Beneficiario()
                        {
                            CPF = modelBenef.CPF,
                            Nome = modelBenef.Nome,
                            IdCliente = model.Id
                        };
                        boBenef.Incluir(benef);
                    }
                }

                return Json("Cadastro alterado com sucesso");
            }
        }

        [HttpGet]
        public ActionResult Alterar(long id)
        {
            BoCliente bo = new BoCliente();
            Cliente cliente = bo.Consultar(id);
            Models.ClienteModel model = null;

            BoBeneficiario boBenef = new BoBeneficiario();
            List<Beneficiario> beneficiarios = boBenef.Consultar(cliente.Id);

            List<BeneficiarioModel> benefM = new List<BeneficiarioModel>();
            foreach(Beneficiario b in beneficiarios)
            {
                BeneficiarioModel bm = new BeneficiarioModel()
                {
                    CPF = b.CPF,
                    Id = b.Id,
                    IdCliente = b.IdCliente,
                    Nome = b.Nome
                };
                benefM.Add(bm);
            }

            if (cliente != null)
            {
                model = new ClienteModel()
                {
                    Id = cliente.Id,
                    CEP = cliente.CEP,
                    Cidade = cliente.Cidade,
                    Email = cliente.Email,
                    Estado = cliente.Estado,
                    Logradouro = cliente.Logradouro,
                    Nacionalidade = cliente.Nacionalidade,
                    Nome = cliente.Nome,
                    Sobrenome = cliente.Sobrenome,
                    Telefone = cliente.Telefone,
                    CPF = cliente.CPF,
                    Beneficiarios = benefM
                };

            
            }

            return View(model);
        }

        [HttpPost]
        public JsonResult ClienteList(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                int qtd = 0;
                string campo = string.Empty;
                string crescente = string.Empty;
                string[] array = jtSorting.Split(' ');

                if (array.Length > 0)
                    campo = array[0];

                if (array.Length > 1)
                    crescente = array[1];

                List<Cliente> clientes = new BoCliente().Pesquisa(jtStartIndex, jtPageSize, campo, crescente.Equals("ASC", StringComparison.InvariantCultureIgnoreCase), out qtd);

                //Return result to jTable
                return Json(new { Result = "OK", Records = clientes, TotalRecordCount = qtd });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
    }
}