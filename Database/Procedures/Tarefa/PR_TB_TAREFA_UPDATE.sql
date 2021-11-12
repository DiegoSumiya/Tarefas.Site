CREATE PROCEDURE [dbo].[PR_TB_TAREFA_UPDATE]
(
	@ID INT,
	@EMAIL VARCHAR(100),
	@DATA DATE,
	@DESCRICAO VARCHAR(100),
	@NOTIFICACAO BIT,
	@IDCATEGORIA INT
	
)
AS
BEGIN
	UPDATE TAREFA SET DATA =        @DATA,
				      DESCRICAO =   @DESCRICAO,
					  NOTIFICACAO = @NOTIFICACAO,
					  IDCATEGORIA = @IDCATEGORIA
	WHERE ID = @ID AND EMAIL_USUARIO = @EMAIL;
END
