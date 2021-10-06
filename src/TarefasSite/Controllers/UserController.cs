using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Tarefas.Dominio.Models;
using Tarefas.Dominio.Repositorio;
using TarefasSite.ViewModels;

namespace TarefasSite.Controllers
{
    public class UserController : Controller
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        public UserController(IUsuarioRepositorio usuarioRepositorio)
        {
            _usuarioRepositorio = usuarioRepositorio;
        }
        
        public IActionResult Index()
        {
            return View();
        }
       
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
       
       
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if(this.ModelState.IsValid)
            {
                Usuario usuario = _usuarioRepositorio.Buscar(loginViewModel.Email);
                
                if(usuario == null || usuario.Senha != loginViewModel.Senha)
                {
                    //erro >> usuario nao encontrado
                    ViewBag.ExisteErro = true;
                    this.ModelState.AddModelError("usuario_senha_invalido", "Usuário ou senha inválidos");
                    return View(loginViewModel);
                }


                // Criar Sessão para o usario
                // Redirecionar para tela de tarefas
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, usuario.Email),
                    new Claim("FullName", usuario.Nome),
                    new Claim(ClaimTypes.Role, "Usuario"),
                };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                   
                    ExpiresUtc = DateTimeOffset.Now.AddMinutes(10),
                    IssuedUtc = DateTime.Now,
                };

                await HttpContext.SignInAsync(
                   CookieAuthenticationDefaults.AuthenticationScheme,
                   new ClaimsPrincipal(claimsIdentity),
                   authProperties);

                return RedirectToAction("Index", "Tarefas");
            }
            else
            {
                ViewBag.ExisteErro = true;
            }
            return View(loginViewModel);
        }
       
        [HttpGet]
        public IActionResult Nova()
        {
            return View();
        }
        
        [HttpPost]
        public IActionResult Nova(NovoUsuarioViewModel novoUsuarioViewModel)
        {
            if (this.ModelState.IsValid)
            {
                if(novoUsuarioViewModel.Senha != novoUsuarioViewModel.ConfirmarSenha)
                {
                    ViewBag.ExisteErro = true;
                    this.ModelState.AddModelError("senha_diferente_confirma_senha", "Senha e confirmar senha devem ser iguais");
                    return View(novoUsuarioViewModel);
                }
                
                Usuario usuario = _usuarioRepositorio.Buscar(novoUsuarioViewModel.Email);

                if(usuario != null)
                {
                    ViewBag.ExisteErro = true;
                    this.ModelState.AddModelError("usuario_ja_existe", "Email já existente");
                    return View(novoUsuarioViewModel);
                }
                
                Usuario novoUsuario = new Usuario(novoUsuarioViewModel.Nome, novoUsuarioViewModel.Email, novoUsuarioViewModel.Senha);
                _usuarioRepositorio.Inserir(novoUsuario);
                return RedirectToAction("Login");
            }
            else
            {
                ViewBag.ExisteErro = true;
            }
            return View(novoUsuarioViewModel);
        }

        public IActionResult Logout()
        {
            return RedirectToAction("Home");
        }
    }
}
