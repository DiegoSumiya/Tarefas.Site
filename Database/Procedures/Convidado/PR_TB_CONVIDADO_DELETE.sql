CREATE PROCEDURE [dbo].[PR_TB_CONVIDADO_DELETE]
(
	@ID INT
	
)
AS
BEGIN
	DELETE FROM CONVIDADO_TAREFA WHERE ID_TAREFA = @ID;
END

