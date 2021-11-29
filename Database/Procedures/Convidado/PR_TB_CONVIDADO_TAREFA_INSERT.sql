USE [TarefaDB]
GO

/****** Object:  StoredProcedure [dbo].[PR_TB_CONVIDADO_TAREFA_INSERT]    Script Date: 20/11/2021 19:12:18 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[PR_TB_CONVIDADO_TAREFA_INSERT]
(
	@EMAIL_CONVIDADO VARCHAR(100),
	@ID_TAREFA INT
)
AS
BEGIN
	INSERT INTO CONVIDADO_TAREFA (EMAIL_CONVIDADO, ID_TAREFA)
	VALUES (@EMAIL_CONVIDADO, @ID_TAREFA);
END
GO


