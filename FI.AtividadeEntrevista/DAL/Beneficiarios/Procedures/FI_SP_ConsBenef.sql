SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[FI_SP_ConsBenef]
    @IDCLIENTE BIGINT
AS
BEGIN
    SELECT * FROM BENEFICIARIOS WITH(NOLOCK) WHERE IDCLIENTE = @IDCLIENTE;
END
GO
