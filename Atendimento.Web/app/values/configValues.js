app.value("config", {
    baseUrl: "http://localhost:51765/api"
})

//Se eu definir com 'constant' 
//consigo injetar essas configurações
//em serviços do tipo provider
app.constant("configConstant", {
    baseUrl: "http://localhost:51765/api"
})