function data()
    return {
      info = {
        name = _("Interactive Vehicle Display"),         
        description = _("Interactive Vehicle Display"),
        authors = {
          {
            name = "Molizo",
            role = 'CREATOR',
          },
        },
        minorVersion = 1, 
        url = "https://github.com/Molizo",         
      },
      postRunFn = function(settings, params)
        print("IVD postRunFn")
      end
    }
end