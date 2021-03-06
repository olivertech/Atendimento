﻿using Atendimento.Entities.Entities;
using Dapper.FluentMap.Dommel.Mapping;

namespace Atendimento.Repository.Mapping
{
    public class TemplateRespostaMapping : DommelEntityMap<TemplateResposta>
    {
        public TemplateRespostaMapping()
        {
            ToTable("Template_Resposta");
            Map(m => m.Id).ToColumn("id_template_repository_entity").IsKey().IsIdentity();
            Map(m => m.Titulo).ToColumn("titulo");
            Map(m => m.Descricao).ToColumn("descricao");
            Map(m => m.Conteudo).ToColumn("conteudo");
        }
    }
}
