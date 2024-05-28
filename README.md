# Sistema de Atendimento/Suporte

O propósito desde repositório é expor um modelo preliminar de sistema de atendimento/suporte para web, desenvolvido com o conceito de SPA para o front-end, acessando back-end de serviços Restfull WebApi.

Não pretendo aqui esgotar o sistema em si, mas apenas ter aqui uma amostra de parte do projeto, com parte do código fonte do front-end, e parte do código do back-end. 

A parte do front-end foi desenvolvida com Angularjs 1.75, JQuery, Html 5, Css 3 e Bootstrap 4. O código ainda não contém a minificação de libs nem mesmo o bundling de todos os códigos em javascript, pois a ideia é mostrar o código desenvolvido, com a possibilidade de poder avaliar controllers, services, interceptors, routes etc. Para o layout, foi utilizado um template pago, adquirido no site https://themeforest.net/.

A parte do back-end foi desenvolvida com Asp.Net MVC 5 WebApi 2 (com C#), com uma arquitetura em camadas, com diversos recursos para permitir um desenvolvimento com baixo acoplamento, e melhor isolamento de classes e métodos. Foram empregados no projeto, Swagger UI para uso na documentação da api e para auxiliar nos testes dos serviços, recurso de mapeamento de classes por meio do Automapper, recurso de injeção de dependência com MVC Unity 5, recurso de autenticação com Json Web Token com geração dinâmica de Token, recurso de mapeamento objeto relacional (ORM), com Dapper ORM, para a parte de acesso aos repositórios, além do banco de dados SQL Server, não estando a parte da modelagem e entidades do banco, exposto nesse repositório.

Espero com esse repositório, apresentar um pouco daquilo que uso em meus projetos, não querendo de forma alguma declarar qualquer arquitetura ou abordagem aqui apresentada, como sendo a melhor ou a mais indicada. O projeto aqui proposto é apenas uma versão preliminar, contendo um caminho que pode ser seguido, ou mesmo usado de referência para outras abordagens mais modernas e atuais. 

Por questões de segurança, foram removidas algumas partes do código, sem comprometer o entendimento, para que se preserve os clientes que estão utilizando este projeto em seus ambientes de produção.

Para maiores informações, ou contato profissional, me coloco a disposição pelo email olivertech@outlook.com, pelo celular (21) 99710-8994 (Ligações / WhatsApp) ou pelo Skype maclauservicos.

Marcelo de Oliveira.
