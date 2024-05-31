using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Pautas.Models;
using Pautas.Models.Pautas;
using Pautas.Services.Conection;
using Pautas.Services.Extensions;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Pautas.Services.Pauta
{
    public class PautaServices
    {

        ConnectionDb _connService = new ConnectionDb();

        #region StatusSelect
        public List<StatusPautaModel> SP_STATUS_SELECT()
        {
            List<StatusPautaModel> olista = new List<StatusPautaModel>();
            //try
            //{
            using (SqlConnection sql = new SqlConnection(_connService.stringSqlPautaDb()))
            {
                using (SqlCommand cmd = new SqlCommand("p_PT_StatusSelect", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    sql.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var obj = new StatusPautaModel()
                            {
                                STATUS_ID = dr.GetInt32(dr.GetOrdinal("STATUS_ID")),
                                STATUS = dr.GetString(dr.GetOrdinal("STATUS")),
                            };
                            olista.Add(obj);
                        }
                    }
                }
            }
            //}
            //catch (Exception ex)
            //{

            //    olista.Add(new ProfitModel
            //    {
            //        code = "error",
            //        message = ex.Message
            //    });
            //}
            return olista;
        }
        #endregion

        #region Statusfinish
        public List<PautaModel> SP_STATUS_FINISH()
        {
            List<PautaModel> olista = new List<PautaModel>();
            //try
            //{
            using (SqlConnection sql = new SqlConnection(_connService.stringSqlPautaDb()))
            {
                using (SqlCommand cmd = new SqlCommand("p_PT_StatusFinish", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    sql.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var obj = new PautaModel()
                            {
                                PAUTA_ID = dr.GetInt32(dr.GetOrdinal("PAUTA_ID")),
                                STATUS_ID = dr.GetInt32Null(dr.GetOrdinal("STATUS_ID")),
                                STATUS = dr.GetStringNull(dr.GetOrdinal("STATUS")),
                                ANNO = dr.GetInt32Null(dr.GetOrdinal("ANNO")),
                                START_DATE = dr.GetString(dr.GetOrdinal("START_DATE")),
                                END_DATE = dr.GetString(dr.GetOrdinal("END_DATE")),
                                CAMPANNA = dr.GetStringNull(dr.GetOrdinal("CAMPANNA")),
                                SALE = dr.GetDecimalNull(dr.GetOrdinal("SALE")),
                                UTILITY = dr.GetDecimalNull(dr.GetOrdinal("UTILITY")),
                                MARGIN = dr.GetDecimalNull(dr.GetOrdinal("MARGIN")),
                                PRODUCT_NAME = dr.GetStringNull(dr.GetOrdinal("PRODUCT_NAME")),
                                SCOPE_RRSS = dr.GetDecimalNull(dr.GetOrdinal("SCOPE_RRSS")),
                                COST_RESULT = dr.GetDecimalNull(dr.GetOrdinal("COST_RESULT")),
                                INV_ADS = dr.GetDecimalNull(dr.GetOrdinal("INV_ADS")),
                                ROI = dr.GetDecimalNull(dr.GetOrdinal("ROI")),
                                MONTH_ID = dr.GetInt32Null(dr.GetOrdinal("MONTH_ID")),
                                STORE_NAME = dr.GetStringNull(dr.GetOrdinal("STORE_NAME")),
                                PC_ID = dr.GetInt32Null(dr.GetOrdinal("PC_ID")),
                                PC_NAME = dr.GetStringNull(dr.GetOrdinal("PC_NAME")),
                                SALE_PIECE = dr.GetInt32Null(dr.GetOrdinal("SALE_PIECE"))
                            };
                            olista.Add(obj);
                        }
                    }
                }
            }

            return olista;
        }
        #endregion

        #region StatusBegin
        public List<PautaModel> SP_STATUS_BEGIN()
        {
            List<PautaModel> olista = new List<PautaModel>();
            //try
            //{
            using (SqlConnection sql = new SqlConnection(_connService.stringSqlPautaDb()))
            {
                using (SqlCommand cmd = new SqlCommand("p_PT_StatusBegin", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    sql.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var obj = new PautaModel()
                            {
                                PAUTA_ID = dr.GetInt32(dr.GetOrdinal("PAUTA_ID")),
                                STATUS_ID = dr.GetInt32Null(dr.GetOrdinal("STATUS_ID")),
                                STATUS = dr.GetStringNull(dr.GetOrdinal("STATUS")),
                                ANNO = dr.GetInt32Null(dr.GetOrdinal("ANNO")),
                                START_DATE = dr.GetString(dr.GetOrdinal("START_DATE")),
                                END_DATE = dr.GetString(dr.GetOrdinal("END_DATE")),
                                CAMPANNA = dr.GetStringNull(dr.GetOrdinal("CAMPANNA")),
                                SALE = dr.GetDecimalNull(dr.GetOrdinal("SALE")),
                                UTILITY = dr.GetDecimalNull(dr.GetOrdinal("UTILITY")),
                                MARGIN = dr.GetDecimalNull(dr.GetOrdinal("MARGIN")),
                                PRODUCT_NAME = dr.GetStringNull(dr.GetOrdinal("PRODUCT_NAME")),
                                SCOPE_RRSS = dr.GetDecimalNull(dr.GetOrdinal("SCOPE_RRSS")),
                                COST_RESULT = dr.GetDecimalNull(dr.GetOrdinal("COST_RESULT")),
                                INV_ADS = dr.GetDecimalNull(dr.GetOrdinal("INV_ADS")),
                                ROI = dr.GetDecimalNull(dr.GetOrdinal("ROI")),
                                MONTH_ID = dr.GetInt32Null(dr.GetOrdinal("MONTH_ID")),
                                STORE_NAME = dr.GetStringNull(dr.GetOrdinal("STORE_NAME")),
                                PC_ID = dr.GetInt32Null(dr.GetOrdinal("PC_ID")),
                                PC_NAME = dr.GetStringNull(dr.GetOrdinal("PC_NAME")),
                                SALE_PIECE = dr.GetInt32Null(dr.GetOrdinal("SALE_PIECE"))
                            };
                            olista.Add(obj);
                        }
                    }
                }
            }

            return olista;
        }
        #endregion

        #region StatusInProcess
        public List<PautaModel> SP_STATUS_IN_PROCESS()
        {
            List<PautaModel> olista = new List<PautaModel>();
            //try
            //{
            using (SqlConnection sql = new SqlConnection(_connService.stringSqlPautaDb()))
            {
                using (SqlCommand cmd = new SqlCommand("p_PT_StatusInProcess", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    sql.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var obj = new PautaModel()
                            {
                                PAUTA_ID = dr.GetInt32(dr.GetOrdinal("PAUTA_ID")),
                                STATUS_ID = dr.GetInt32Null(dr.GetOrdinal("STATUS_ID")),
                                STATUS = dr.GetStringNull(dr.GetOrdinal("STATUS")),
                                ANNO = dr.GetInt32Null(dr.GetOrdinal("ANNO")),
                                START_DATE = dr.GetString(dr.GetOrdinal("START_DATE")),
                                END_DATE = dr.GetString(dr.GetOrdinal("END_DATE")),
                                CAMPANNA = dr.GetStringNull(dr.GetOrdinal("CAMPANNA")),
                                SALE = dr.GetDecimalNull(dr.GetOrdinal("SALE")),
                                UTILITY = dr.GetDecimalNull(dr.GetOrdinal("UTILITY")),
                                MARGIN = dr.GetDecimalNull(dr.GetOrdinal("MARGIN")),
                                PRODUCT_NAME = dr.GetStringNull(dr.GetOrdinal("PRODUCT_NAME")),
                                SCOPE_RRSS = dr.GetDecimalNull(dr.GetOrdinal("SCOPE_RRSS")),
                                COST_RESULT = dr.GetDecimalNull(dr.GetOrdinal("COST_RESULT")),
                                INV_ADS = dr.GetDecimalNull(dr.GetOrdinal("INV_ADS")),
                                ROI = dr.GetDecimalNull(dr.GetOrdinal("ROI")),
                                MONTH_ID = dr.GetInt32Null(dr.GetOrdinal("MONTH_ID")),
                                STORE_NAME = dr.GetStringNull(dr.GetOrdinal("STORE_NAME")),
                                PC_ID = dr.GetInt32Null(dr.GetOrdinal("PC_ID")),
                                PC_NAME = dr.GetStringNull(dr.GetOrdinal("PC_NAME")),
                                SALE_PIECE = dr.GetInt32Null(dr.GetOrdinal("SALE_PIECE"))
                            };
                            olista.Add(obj);
                        }
                    }
                }
            }

            return olista;
        }
        #endregion

        #region ProfitSelect
        public List<ProfitModel> SP_PROFIT_SELECT()
        {
            List<ProfitModel> olista = new List<ProfitModel>();
            //try
            //{
            using (SqlConnection sql = new SqlConnection(_connService.stringSqlPautaDb()))
            {
                using (SqlCommand cmd = new SqlCommand("p_PT_ProfitSelect", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    sql.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var obj = new ProfitModel()
                            {
                                PC_ID = dr.GetInt32(dr.GetOrdinal("PC_ID")),
                                PC_NAME = dr.GetString(dr.GetOrdinal("PC_NAME")),
                            };
                            olista.Add(obj);
                        }
                    }
                }
            }
            //}
            //catch (Exception ex)
            //{

            //    olista.Add(new ProfitModel
            //    {
            //        code = "error",
            //        message = ex.Message
            //    });
            //}
            return olista;
        }
        #endregion

        #region StoreSelect
        public List<StoreModel> SP_STORE_SELECT()
        {
            List<StoreModel> olista = new List<StoreModel>();
            try
            {
                using (SqlConnection sql = new SqlConnection(_connService.stringSqlPautaDb()))
                {
                    using (SqlCommand cmd = new SqlCommand("p_PT_StoreSelect", sql))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        sql.Open();

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                var obj = new StoreModel()
                                {
                                    STORE_CODE = dr.GetInt32(dr.GetOrdinal("STORE_CODE")),
                                    STORE_NAME = dr.GetString(dr.GetOrdinal("STORE_NAME")),
                                    STORE_ADDRESS = dr.GetString(dr.GetOrdinal("STORE_ADDRESS"))
                                };
                                olista.Add(obj);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                olista.Add(new StoreModel
                {
                    code = "error",
                    message = ex.Message
                });
            }
            return olista;
        }
        #endregion

        #region Pautas Select By One
        public PautaModel SP_PAUTAS_SELECTBYONE(int PAUTA_ID)
        {
            PautaModel oPauta = new PautaModel();
            try
            {
                using (SqlConnection sql = new SqlConnection(_connService.stringSqlPautaDb()))
                {
                    using (SqlCommand cmd = new SqlCommand("p_PT_PautasSelectByOne", sql))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@PAUTA_ID", SqlDbType.Int).Value = PAUTA_ID;

                        sql.Open();

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                var obj = new PautaModel()
                                {
                                    PAUTA_ID = dr.GetInt32(dr.GetOrdinal("PAUTA_ID")),
                                    STATUS_ID = dr.GetInt32Null(dr.GetOrdinal("STATUS_ID")),
                                    STATUS = dr.GetStringNull(dr.GetOrdinal("STATUS")),
                                    ANNO = dr.GetInt32Null(dr.GetOrdinal("ANNO")),
                                    START_DATE = dr.GetString(dr.GetOrdinal("START_DATE")),
                                    END_DATE = dr.GetString(dr.GetOrdinal("END_DATE")),
                                    CAMPANNA = dr.GetStringNull(dr.GetOrdinal("CAMPANNA")),
                                    SALE = dr.GetDecimalNull(dr.GetOrdinal("SALE")),
                                    UTILITY = dr.GetDecimalNull(dr.GetOrdinal("UTILITY")),
                                    MARGIN = dr.GetDecimalNull(dr.GetOrdinal("MARGIN")),
                                    PRODUCT_NAME = dr.GetStringNull(dr.GetOrdinal("PRODUCT_NAME")),
                                    SCOPE_RRSS = dr.GetDecimalNull(dr.GetOrdinal("SCOPE_RRSS")),
                                    COST_RESULT = dr.GetDecimalNull(dr.GetOrdinal("COST_RESULT")),
                                    INV_ADS = dr.GetDecimalNull(dr.GetOrdinal("INV_ADS")),
                                    ROI = dr.GetDecimalNull(dr.GetOrdinal("ROI")),
                                    MONTH_ID = dr.GetInt32Null(dr.GetOrdinal("MONTH_ID")),
                                    STORE_NAME = dr.GetStringNull(dr.GetOrdinal("STORE_NAME")),
                                    PC_ID = dr.GetInt32Null(dr.GetOrdinal("PC_ID")),
                                    PC_NAME = dr.GetStringNull(dr.GetOrdinal("PC_NAME")),
                                    SALE_PIECE = dr.GetInt32Null(dr.GetOrdinal("SALE_PIECE"))
                                };
                                oPauta = obj;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return oPauta;
        }
        #endregion

        #region Pautas Select
        public List<PautaModel> SP_PAUTAS_SELECT()
        {
            List<PautaModel> olista = new List<PautaModel>();
            try
            {
                using (SqlConnection sql = new SqlConnection(_connService.stringSqlPautaDb()))
                {
                    using (SqlCommand cmd = new SqlCommand("p_PT_PautasSelect", sql))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        sql.Open();

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                var obj = new PautaModel()
                                {
                                    PAUTA_ID = dr.GetInt32(dr.GetOrdinal("PAUTA_ID")),
                                    STATUS_ID = dr.GetInt32Null(dr.GetOrdinal("STATUS_ID")),
                                    STATUS = dr.GetStringNull(dr.GetOrdinal("STATUS")),
                                    ANNO = dr.GetInt32Null(dr.GetOrdinal("ANNO")),
                                    START_DATE = dr.GetString(dr.GetOrdinal("START_DATE")),
                                    END_DATE = dr.GetString(dr.GetOrdinal("END_DATE")),
                                    CAMPANNA = dr.GetStringNull(dr.GetOrdinal("CAMPANNA")),
                                    SALE = dr.GetDecimalNull(dr.GetOrdinal("SALE")),
                                    UTILITY = dr.GetDecimalNull(dr.GetOrdinal("UTILITY")),
                                    MARGIN = dr.GetDecimalNull(dr.GetOrdinal("MARGIN")),
                                    PRODUCT_NAME = dr.GetStringNull(dr.GetOrdinal("PRODUCT_NAME")),
                                    SCOPE_RRSS = dr.GetDecimalNull(dr.GetOrdinal("SCOPE_RRSS")),
                                    COST_RESULT = dr.GetDecimalNull(dr.GetOrdinal("COST_RESULT")),
                                    INV_ADS = dr.GetDecimalNull(dr.GetOrdinal("INV_ADS")),
                                    ROI = dr.GetDecimalNull(dr.GetOrdinal("ROI")),
                                    MONTH_ID = dr.GetInt32Null(dr.GetOrdinal("MONTH_ID")),
                                    STORE_NAME = dr.GetStringNull(dr.GetOrdinal("STORE_NAME")),
                                    PC_ID = dr.GetInt32Null(dr.GetOrdinal("PC_ID")),
                                    PC_NAME = dr.GetStringNull(dr.GetOrdinal("PC_NAME")),
                                    SALE_PIECE = dr.GetInt32Null(dr.GetOrdinal("SALE_PIECE"))
                                };
                                olista.Add(obj);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejo de excepciones aquí
                // Puedes registrar el error, lanzar una excepción o manejarlo de otra manera
                // Por ejemplo, podrías asignar un mensaje de error al modelo y devolverlo
                olista.Add(new PautaModel { code = "error", message = ex.Message });
            }
            return olista;
        }
        #endregion

        #region Pautas Edit
        public GenericResponse SP_PAUTAS_EDIT(PautaModel model)
        {
            GenericResponse resp = new GenericResponse();
            int PautaId = 0;

            try
            {
                using (SqlConnection sql = new SqlConnection(_connService.stringSqlPautaDb()))
                {
                    using (SqlCommand cmd = new SqlCommand("p_PT_PautasEdit", sql))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@PAUTA_ID", SqlDbType.Int).Value = model.PAUTA_ID;
                        cmd.Parameters.Add("@ANNO", SqlDbType.Int).Value = model.ANNO;
                        cmd.Parameters.Add("@START_DATE", SqlDbType.DateTime).Value = model.START_DATE;
                        cmd.Parameters.Add("@END_DATE", SqlDbType.DateTime).Value = model.END_DATE;
                        cmd.Parameters.Add("@CAMPANNA", SqlDbType.NVarChar).Value = model.CAMPANNA;
                        cmd.Parameters.Add("@PRODUCT_NAME", SqlDbType.NVarChar).Value = model.PRODUCT_NAME;
                        cmd.Parameters.Add("@SCOPE_RRSS", SqlDbType.Decimal).Value = model.SCOPE_RRSS;
                        cmd.Parameters.Add("@USER_UPDATE", SqlDbType.NVarChar).Value = model.USER_UPDATE;
                        cmd.Parameters.Add("@COST_RESULT", SqlDbType.Decimal).Value = model.COST_RESULT;
                        cmd.Parameters.Add("@INV_ADS", SqlDbType.Decimal).Value = model.INV_ADS;
                        cmd.Parameters.Add("@MONTH_ID", SqlDbType.Int).Value = model.MONTH_ID;
                        cmd.Parameters.Add("@PC_ID", SqlDbType.Int).Value = model.PC_ID;

                        SqlParameter spMessage = new SqlParameter("@MESSAGE", SqlDbType.NVarChar, -1);
                        spMessage.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(spMessage);

                        SqlParameter spResponse = new SqlParameter("@RESPONSE", SqlDbType.Bit);
                        spResponse.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(spResponse);

                        sql.Open();
                        cmd.ExecuteNonQuery();

                        PautaId = model.PAUTA_ID;

                        resp.id = PautaId;
                        resp.message = spMessage.Value.ToString();
                        resp.Resp = Convert.ToBoolean(spResponse.Value);

                        if (resp.Resp)
                        {
                            //Eliminar el registro de la pauta en la tienda
                            SP_PAUTAS_DELETE_STORE(PautaId);

                            //Modifica el estado de la pauta
                            SP_PAUTAS_UPDATE_STATUS(PautaId, 2);

                            var tiendas = model.STORE_CODE.ToList();
                            foreach (var tiendaId in tiendas)
                            {
                                // Inserta la tienda en la pauta 
                                SP_PAUTAS_INSERTSTORE(PautaId, tiendaId);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                resp.code = "error";
                resp.message = ex.Message;
                resp.Resp = false;
            }
            return resp;
        }
        #endregion

        #region Pautas Create
        public GenericResponse SP_PAUTAS_CREATE(PautaModel model)
        {
            GenericResponse resp = new GenericResponse();
            int PautaId = 0;
            try
            {
                using (SqlConnection sql = new SqlConnection(_connService.stringSqlPautaDb()))
                {
                    using (SqlCommand cmd = new SqlCommand("p_PT_PautasCreate", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ANNO", model.ANNO);
                        cmd.Parameters.AddWithValue("@START_DATE", Convert.ToDateTime(model.START_DATE));
                        cmd.Parameters.AddWithValue("@END_DATE", Convert.ToDateTime(model.END_DATE));
                        cmd.Parameters.AddWithValue("@CAMPANNA", model.CAMPANNA);
                        cmd.Parameters.AddWithValue("@PRODUCT_NAME", model.PRODUCT_NAME);
                        cmd.Parameters.AddWithValue("@SCOPE_RRSS", model.SCOPE_RRSS);
                        cmd.Parameters.AddWithValue("@COST_RESULT", model.COST_RESULT);
                        cmd.Parameters.AddWithValue("@INV_ADS", model.INV_ADS);
                        cmd.Parameters.AddWithValue("@MONTH_ID", model.MONTH_ID);
                        cmd.Parameters.AddWithValue("@PC_ID", model.PC_ID);

                        cmd.Parameters.AddWithValue("@USER_CREATE", model.USER_CREATE);

                        SqlParameter spPautaId = new SqlParameter("@PAUTA_ID", SqlDbType.Int);
                        spPautaId.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(spPautaId);

                        SqlParameter spMessage = new SqlParameter("@MESSAGE", SqlDbType.NVarChar);
                        spMessage.Size = -1;
                        spMessage.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(spMessage);

                        SqlParameter spResponse = new SqlParameter("@RESPONSE", SqlDbType.Bit);
                        spResponse.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(spResponse);

                        sql.Open();
                        cmd.ExecuteScalar();
                        PautaId = Convert.ToInt32(spPautaId.Value);

                        resp.id = PautaId;
                        resp.message = spMessage.Value.ToString();
                        resp.Resp = Convert.ToBoolean(spResponse.Value);

                    }
                }

                if (resp.Resp)
                {
                    var tiendas = model.STORE_CODE.ToList();
                    foreach (var tiendaId in tiendas)
                    {
                        SP_PAUTAS_INSERTSTORE(PautaId, tiendaId);
                    }
                }
            }
            catch (Exception ex)
            {
                resp.id = 0;
                resp.Resp = false;
                resp.code = "error";
                resp.message = ex.Message;
            }

            return resp;
        }

        #endregion

        #region List Product By One
        public List<ProductModel> SP_PRODUCT_SELECT_DETAILS(int PAUTA_ID)
        {
            List<ProductModel> olista = new List<ProductModel>();

            try
            {
                using (SqlConnection sql = new SqlConnection(_connService.stringSqlPautaDb()))
                {
                    using (SqlCommand cmd = new SqlCommand("p_PT_PautasDetailSelect", sql))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@PAUTA_ID", SqlDbType.Int).Value = PAUTA_ID;

                        sql.Open();

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                var obj = new ProductModel()
                                {
                                    PAUTA_ID = dr.GetInt32(dr.GetOrdinal("PAUTA_ID")),
                                    PRODUCT_CODE = dr.GetInt32(dr.GetOrdinal("PRODUCT_CODE")),
                                    PROD_DESC = dr.GetString(dr.GetOrdinal("PROD_DESC")),
                                    QTY = dr.GetInt32Null(dr.GetOrdinal("QTY")),
                                    SALE = dr.GetDecimalNull(dr.GetOrdinal("SALE")),
                                    UTILITY = dr.GetDecimal(dr.GetOrdinal("UTILITY")),
                                    MARGIN = dr.GetDecimalNull(dr.GetOrdinal("MARGIN")),
                                    TRANSNUM = dr.GetInt32Null(dr.GetOrdinal("TRANSNUM")),
                                    TICKET_PROM = dr.GetDecimalNull(dr.GetOrdinal("TICKET_PROM")),
                                };
                                olista.Add(obj);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Aquí puedes manejar la excepción de acuerdo a tus necesidades.
                // Por ejemplo, podrías lanzarla nuevamente o registrarla.
                // En este ejemplo, simplemente se agrega un objeto ProductModel con el mensaje de error.
                olista.Add(new ProductModel { code = "error", message = ex.Message });
            }
            return olista;
        }
        #endregion

        #region Product Select
        public List<ProductModel> SP_PRODUCT_SELECT()
        {
            List<ProductModel> olista = new List<ProductModel>();

            try
            {
                using (SqlConnection sql = new SqlConnection(_connService.stringSqlPautaDb()))
                {
                    using (SqlCommand cmd = new SqlCommand("p_PT_ProductSelect", sql))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        sql.Open();


                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                var obj = new ProductModel()
                                {
                                    PRODUCT_CODE = dr.GetInt32(dr.GetOrdinal("PRODUCT_CODE")),
                                    SUPPLIER_CODE = dr.GetStringNull(dr.GetOrdinal("SUPPLIER_CODE")),
                                    PROD_DESC = dr.GetStringNull(dr.GetOrdinal("PROD_DESC")),
                                    PRICE_RETAIL = dr.GetDecimalNull(dr.GetOrdinal("PRICE_RETAIL")),
                                    PHOTO_PATH = dr.GetStringNull(dr.GetOrdinal("PHOTO_PATH")),

                                };
                                olista.Add(obj);
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                // Manejo de excepciones aquí
                // Puedes registrar el error, lanzar una excepción o manejarlo de otra manera
                // Por ejemplo, podrías asignar un mensaje de error al modelo y devolverlo
            }
            return olista;
        }
        #endregion

        #region Insert Product
        public ProductModel SP_PAUTAS_PRODUCT_INSERT(int PAUTA_ID, int PRODUCT_CODE)
        {
            ProductModel resp = new ProductModel();

            try
            {
                using (SqlConnection connection = new SqlConnection(_connService.stringSqlPautaDb()))
                {
                    using (SqlCommand command = new SqlCommand("p_PT_PautasProductInsert", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Add("@PAUTA_ID", SqlDbType.Int).Value = PAUTA_ID;
                        command.Parameters.Add("@PRODUCT_CODE", SqlDbType.Int).Value = PRODUCT_CODE;

                        SqlParameter messageParam = new SqlParameter("@MESSAGE", SqlDbType.NVarChar, -1);
                        messageParam.Direction = ParameterDirection.Output;
                        command.Parameters.Add(messageParam);

                        SqlParameter responseParam = new SqlParameter("@RESPONSE", SqlDbType.Bit);
                        responseParam.Direction = ParameterDirection.Output;
                        command.Parameters.Add(responseParam);

                        connection.Open();
                        command.ExecuteNonQuery();
                        resp.id = PAUTA_ID;
                        resp.message = messageParam.Value.ToString();
                        resp.Resp = Convert.ToBoolean(responseParam.Value);
                    }
                }
            }
            catch (Exception ex)
            {
                resp.id = PAUTA_ID;
                resp.code = "error";
                resp.message = ex.Message;
            }

            return resp;
        }
        #endregion

        #region Product Select By One

        public ProductModel SP_PRODUCT_SELECT_BYONE(int PRODUCT_CODE)
        {
            ProductModel oProduct = new ProductModel();

            try
            {
                using (SqlConnection sql = new SqlConnection(_connService.stringSqlPautaDb()))
                {
                    using (SqlCommand cmd = new SqlCommand("p_PT_ProductSelect", sql))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@PRODUCT_CODE", SqlDbType.Int).Value = PRODUCT_CODE;

                        sql.Open();


                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                var obj = new ProductModel()
                                {
                                    PRODUCT_CODE = dr.GetInt32Null(dr.GetOrdinal("PRODUCT_CODE")),
                                    SUPPLIER_CODE = dr.GetStringNull(dr.GetOrdinal("SUPPLIER_CODE")),
                                    PROD_DESC = dr.GetStringNull(dr.GetOrdinal("PROD_DESC")),
                                    PRICE_RETAIL = dr.GetDecimalNull(dr.GetOrdinal("PRICE_RETAIL")),
                                    PHOTO_PATH = dr.GetStringNull(dr.GetOrdinal("PHOTO_PATH")),

                                };
                                oProduct = obj;
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                // Manejo de excepciones aquí
                // Puedes registrar el error, lanzar una excepción o manejarlo de otra manera
                // Por ejemplo, podrías asignar un mensaje de error al modelo y devolverlo
            }
            return oProduct;
            ;
        }
#endregion

        #region Pautas InsertImagen
        public PautaModel SP_IMAGEN_INSERT(PautaModel model)
        {
            PautaModel resp = new PautaModel();
            try
            {
                using (SqlConnection sql = new SqlConnection(_connService.stringSqlPautaDb()))
                {
                    using (SqlCommand cmd = new SqlCommand("p_PT_PautasImagenInsert", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@PAUTA_ID", model.PAUTA_ID);
                        cmd.Parameters.AddWithValue("@IMAGES_ID", model.IMAGES_ID);
                        cmd.Parameters.AddWithValue("@URL", model.URL);
                        cmd.Parameters.AddWithValue("@IMAGES_NAME", model.IMAGES_NAME);

                        SqlParameter spMessage = new SqlParameter("@MESSAGE", SqlDbType.NVarChar, -1);
                        spMessage.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(spMessage);

                        SqlParameter spResponse = new SqlParameter("@RESPONSE", SqlDbType.Bit);
                        spResponse.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(spResponse);

                        sql.Open();
                        cmd.ExecuteNonQuery();
                        resp.id = model.PAUTA_ID;
                        resp.message = spMessage.Value.ToString();
                        resp.Resp = Convert.ToBoolean(spResponse.Value);
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exception
                resp.id = model.PAUTA_ID;
                resp.Resp = false;
                resp.code = "error";
                resp.message = ex.Message;
            }

            return resp;
        }

        #endregion

        #region Pautas DeleteImagen
        public GenericResponse p_PT_Pautas_Imagen_Delete_ByOne(int ID)
        {
            GenericResponse resp = new GenericResponse();
            try
            {
                using (SqlConnection sql = new SqlConnection(_connService.stringSqlPautaDb()))

                {
                    using (SqlCommand cmd = new SqlCommand("p_PT_PautasImagenDeleteByOne", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.Int).Value = ID;

                        SqlParameter messageParam = new SqlParameter("@MESSAGE", SqlDbType.NVarChar, -1);
                        messageParam.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(messageParam);

                        SqlParameter responseParam = new SqlParameter("@RESPONSE", SqlDbType.Bit);
                        responseParam.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(responseParam);

                        sql.Open();
                        cmd.ExecuteNonQuery();

                        resp.message = messageParam.Value.ToString();
                        resp.code = "success";
                    }
                }
            }
            catch (Exception ex)
            {
                resp.Resp = false;
                resp.code = "error";
                resp.message = ex.Message;
            }
            return resp;
        }

        #endregion

        #region List Image By One
        public List<ImageModel> SP_IMAGEN_SELECT(int PAUTA_ID)
        {
            List<ImageModel> olista = new List<ImageModel>();

            try
            {
                using (SqlConnection sql = new SqlConnection(_connService.stringSqlPautaDb()))
                {
                    using (SqlCommand cmd = new SqlCommand("p_PT_ImageDetailSelect", sql))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@PAUTA_ID", SqlDbType.Int).Value = PAUTA_ID;


                        sql.Open();

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                var obj = new ImageModel()
                                {
                                    ID = dr.GetInt32(dr.GetOrdinal("ID")),
                                    IMAGES_ID = dr.GetInt32(dr.GetOrdinal("IMAGES_ID")),

                                    PAUTA_ID = dr.GetInt32(dr.GetOrdinal("PAUTA_ID")),
                                    URL = dr.GetStringNull(dr.GetOrdinal("URL")),
                                    IMAGES_NAME = dr.GetStringNull(dr.GetOrdinal("IMAGES_NAME")),


                                };
                                olista.Add(obj);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejo de excepciones aquí
                // Puedes registrar el error, lanzar una excepción o manejarlo de otra manera
                // Por ejemplo, podrías asignar un mensaje de error al modelo y devolverlo
                olista.Add(new ImageModel { code = "error", message = ex.Message });
            }
            return olista;
        }
        #endregion

        #region Meses Select
        public List<MonthModel> SP_Month_SELECT()
        {
            List<MonthModel> months = new List<MonthModel>();
            try
            {
                using (SqlConnection sql = new SqlConnection(_connService.stringSqlPautaDb()))
                {
                    using (SqlCommand cmd = new SqlCommand("p_PT_MonthSelect", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        sql.Open();

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                var month = new MonthModel()
                                {
                                    MONTH_ID = Convert.ToInt32(dr["MONTH_ID"]),
                                    NAME_MONTH = dr["NAME_MONTH"].ToString()
                                };
                                months.Add(month);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores
                Console.WriteLine("Error al llamar al procedimiento almacenado: " + ex.Message);
            }
            return months;
        }
        #endregion

        #region DropdownMeses
        public List<SelectListItem> DropdownMeses()
        {
            var meses = new List<SelectListItem>
    {
        new SelectListItem { Text = "Enero", Value = "1" },
        new SelectListItem { Text = "Febrero", Value = "2" },
        new SelectListItem { Text = "Marzo", Value = "3" },
        new SelectListItem { Text = "Abril", Value = "4" },
        new SelectListItem { Text = "Mayo", Value = "5" },
        new SelectListItem { Text = "Junio", Value = "6" },
        new SelectListItem { Text = "Julio", Value = "7" },
        new SelectListItem { Text = "Agosto", Value = "8" },
        new SelectListItem { Text = "Septiembre", Value = "9" },
        new SelectListItem { Text = "Octubre", Value = "10" },
        new SelectListItem { Text = "Noviembre", Value = "11" },
        new SelectListItem { Text = "Diciembre", Value = "12" }
    };

            return meses;
        }
        #endregion

        #region ProfitCenterDropdown
        public List<SelectListItem> ProfitCenterDropdown()
        {
            List<SelectListItem> oList = new List<SelectListItem>();

            var listPc = SP_PROFIT_SELECT();

            foreach (var Pc in listPc)
            {
                oList.Add(new SelectListItem
                {
                    Text = Pc.PC_NAME,
                    Value = Pc.PC_ID.ToString()
                });
            }

            return oList;
        }
        #endregion

        #region CountPautas
        public (int, int, int, int) SP_COUNT_PAUTAS()
        {
            int COUNT_TOTAL = 0;
            int COUNT_INICIADO = 0;
            int COUNT_PROCESO = 0;
            int COUNT_TERMINADO = 0;

            try
            {
                using (SqlConnection sql = new SqlConnection(_connService.stringSqlPautaDb()))
                {
                    using (SqlCommand cmd = new SqlCommand("p_PT_PautasCount", sql))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        //cmd.Parameters.Add("@REGISTER_COUNT", SqlDbType.Int).Value = REGISTER_COUNT;

                        sql.Open();

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {

                                COUNT_TOTAL = dr.GetInt32(dr.GetOrdinal("TOTAL"));
                                COUNT_INICIADO = dr.GetInt32(dr.GetOrdinal("INICIADO"));
                                COUNT_PROCESO = dr.GetInt32(dr.GetOrdinal("PROCESO"));
                                COUNT_TERMINADO = dr.GetInt32(dr.GetOrdinal("TERMINADO"));


                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return (COUNT_TOTAL, COUNT_INICIADO, COUNT_PROCESO, COUNT_TERMINADO);
        }
        #endregion

        #region Pautas InsertStore
        public GenericResponse SP_PAUTAS_INSERTSTORE(int PAUTA_ID, int STORE_CODE)
        {
            GenericResponse resp = new GenericResponse();
            try
            {
                using (SqlConnection sql = new SqlConnection(_connService.stringSqlPautaDb()))
                {
                    using (SqlCommand cmd = new SqlCommand("p_PT_PautasInsertStore", sql))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@PAUTA_ID", SqlDbType.Int).Value = PAUTA_ID;
                        cmd.Parameters.Add("@STORE_CODE", SqlDbType.Int).Value = STORE_CODE;

                        SqlParameter spMessage = new SqlParameter("@MESSAGE", SqlDbType.NVarChar);
                        spMessage.Size = -1;
                        spMessage.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(spMessage);

                        SqlParameter spResponse = new SqlParameter("@RESPONSE", SqlDbType.Bit);
                        spResponse.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(spResponse);

                        sql.Open();
                        cmd.ExecuteNonQuery();

                        resp.Resp = Convert.ToBoolean(spResponse.Value);
                        resp.message = spMessage.Value.ToString();

                    }
                }
            }
            catch (Exception ex)
            {
                resp.Resp = false;
                resp.code = "error";
                resp.message = ex.Message;
            }
            return resp;
        }
        #endregion

        #region Pautas DeleteStore
        public GenericResponse SP_PAUTAS_DELETE_STORE(int PAUTA_ID)
        {
            GenericResponse resp = new GenericResponse();
            try
            {
                using (SqlConnection sql = new SqlConnection(_connService.stringSqlPautaDb()))

                {
                    using (SqlCommand cmd = new SqlCommand("p_PT_PautasDeleteStore", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@PAUTA_ID", SqlDbType.Int).Value = PAUTA_ID;

                        SqlParameter messageParam = new SqlParameter("@MESSAGE", SqlDbType.NVarChar, -1);
                        messageParam.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(messageParam);

                        SqlParameter responseParam = new SqlParameter("@RESPONSE", SqlDbType.Bit);
                        responseParam.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(responseParam);

                        sql.Open();
                        cmd.ExecuteNonQuery();

                        resp.message = messageParam.Value.ToString();
                        resp.code = "success";
                    }
                }
            }
            catch (Exception ex)
            {
                resp.code = "error";
                resp.message = ex.Message;
            }
            return resp;
        }
        #endregion

        #region Pautas DeleteProduct
        public GenericResponse SP_PAUTAS_DELETE_PRODUCT(int PRODUCT_CODE, int PAUTA_ID)
        {
            GenericResponse resp = new GenericResponse();
            try
            {
                using (SqlConnection sql = new SqlConnection(_connService.stringSqlPautaDb()))

                {
                    using (SqlCommand cmd = new SqlCommand("p_PT_ProductDelete", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@PAUTA_ID", SqlDbType.Int).Value = PAUTA_ID;

                        cmd.Parameters.Add("@PRODUCT_CODE", SqlDbType.Int).Value = PRODUCT_CODE;

                        SqlParameter messageParam = new SqlParameter("@MESSAGE", SqlDbType.NVarChar, -1);
                        messageParam.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(messageParam);

                        SqlParameter responseParam = new SqlParameter("@RESPONSE", SqlDbType.Bit);
                        responseParam.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(responseParam);

                        sql.Open();
                        cmd.ExecuteNonQuery();

                        resp.message = messageParam.Value.ToString();
                        resp.code = "success";
                    }
                }
            }
            catch (Exception ex)
            {
                resp.Resp = false;
                resp.code = "error";
                resp.message = ex.Message;
            }
            return resp;
        }

        #endregion

        #region Pautas UpdateStatus
        public GenericResponse SP_PAUTAS_UPDATE_STATUS(int PAUTA_ID, int STATUS_ID)
        {
            GenericResponse resp = new GenericResponse();
            try
            {
                using (SqlConnection sql = new SqlConnection(_connService.stringSqlPautaDb()))
                {
                    using (SqlCommand cmd = new SqlCommand("p_PT_PautasStatusUpdate", sql))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@PAUTA_ID", SqlDbType.Int).Value = PAUTA_ID;
                        cmd.Parameters.Add("@STATUS_ID", SqlDbType.Int).Value = STATUS_ID;

                        SqlParameter spMessage = new SqlParameter("@MESSAGE", SqlDbType.NVarChar);
                        spMessage.Size = -1;
                        spMessage.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(spMessage);

                        SqlParameter spResponse = new SqlParameter("@RESPONSE", SqlDbType.Bit);
                        spResponse.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(spResponse);

                        sql.Open();
                        cmd.ExecuteNonQuery();

                        resp.Resp = Convert.ToBoolean(spResponse.Value);
                        resp.message = spMessage.Value.ToString();

                    }
                }
            }
            catch (Exception ex)
            {
                resp.Resp = false;
                resp.code = "error";
                resp.message = ex.Message;
            }
            return resp;
        }
        #endregion

        #region Pautas UpdateStatus
        public GenericResponse SP_PAUTAS_CANCELED_STATUS(int PAUTA_ID, int STATUS_ID)
        {
            GenericResponse resp = new GenericResponse();
            try
            {
                using (SqlConnection sql = new SqlConnection(_connService.stringSqlPautaDb()))
                {
                    using (SqlCommand cmd = new SqlCommand("p_PT_PautasStatusCanceled", sql))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@PAUTA_ID", SqlDbType.Int).Value = PAUTA_ID;
                        cmd.Parameters.Add("@STATUS_ID", SqlDbType.Int).Value = STATUS_ID;

                        SqlParameter spMessage = new SqlParameter("@MESSAGE", SqlDbType.NVarChar);
                        spMessage.Size = -1;
                        spMessage.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(spMessage);

                        SqlParameter spResponse = new SqlParameter("@RESPONSE", SqlDbType.Bit);
                        spResponse.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(spResponse);

                        sql.Open();
                        cmd.ExecuteNonQuery();

                        resp.Resp = Convert.ToBoolean(spResponse.Value);
                        resp.message = spMessage.Value.ToString();

                    }
                }
            }
            catch (Exception ex)
            {
                resp.Resp = false;
                resp.code = "error";
                resp.message = ex.Message;
            }
            return resp;
        }
        #endregion

    }
}
