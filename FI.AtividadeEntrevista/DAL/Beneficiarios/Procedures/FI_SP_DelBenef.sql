SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[FI_SP_DelBenef]
	@ID			BIGINT
AS
BEGIN
	DELETE FROM BENEFICIARIOS WHERE ID = @ID;
END
GO
