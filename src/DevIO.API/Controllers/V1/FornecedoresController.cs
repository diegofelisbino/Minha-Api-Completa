using AutoMapper;
using DevIO.API.Extensions;
using DevIO.API.ViewModels;
using DevIO.Business.Intefaces;
using DevIO.Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevIO.API.Controllers.V1;

[Authorize]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class FornecedoresController : MainController
{
    private readonly IFornecedorRepository _fornecedorRepository;
    private readonly IMapper _mapper;
    private readonly IFornecedorService _fornecedorService;
    private readonly IEnderecoRepository _enderecoRepository;
    private readonly IUser _user;
    public FornecedoresController(IFornecedorRepository fornecedorRepository,
                                  IMapper mapper,
                                  IFornecedorService fornecedorService,
                                  IEnderecoRepository enderecoRepository,
                                  INotificador notificador,
                                  IUser user) : base(notificador, user)
    {
        _fornecedorRepository = fornecedorRepository;
        _mapper = mapper;
        _fornecedorService = fornecedorService;
        _enderecoRepository = enderecoRepository;
        _user = user;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IEnumerable<FornecedorViewModel>> ObterTodos()
    {
        var fornecedores = _mapper.Map<IEnumerable<FornecedorViewModel>>(await _fornecedorRepository.ObterTodos());
        return fornecedores;
    }


    [HttpGet("{id:guid}")]
    public async Task<ActionResult<FornecedorViewModel>> ObterPorId(Guid id)
    {
        var fornecedor = await ObterFornecedorProdutosEndereco(id);

        if (fornecedor == null) return NotFound();

        return Ok(fornecedor);
    }

    [ClaimsAuthorize("Fornecedor", "Adicionar")]
    [HttpPost]
    public async Task<ActionResult> Adicionar(FornecedorViewModel fornecedorViewModel)
    {



        if (!ModelState.IsValid) return CustomResponse(ModelState);

        await _fornecedorService.Adicionar(_mapper.Map<Fornecedor>(fornecedorViewModel));

        return CustomResponse(fornecedorViewModel);
    }

    [ClaimsAuthorize("Fornecedor", "Atualizar")]
    [HttpPut("{id:guid}")]
    public async Task<ActionResult> Atualizar(Guid id, FornecedorViewModel fornecedorViewModel)
    {
        if (id != fornecedorViewModel.Id) return BadRequest();

        if (!ModelState.IsValid) return CustomResponse(ModelState);

        await _fornecedorService.Atualizar(_mapper.Map<Fornecedor>(fornecedorViewModel));

        return CustomResponse(fornecedorViewModel);
    }

    [ClaimsAuthorize("Fornecedor", "Excluir")]
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<FornecedorViewModel>> Excluir(Guid id)
    {

        var fornecedorViewModel = ObterFornecedorEndereco(id);

        if (fornecedorViewModel == null) return NotFound();

        await _fornecedorService.Remover(id);

        return CustomResponse();
    }

    [HttpGet("obter-endereco/{id:guid}")]
    public async Task<EnderecoViewModel> ObterEnderecoPorId(Guid Id)
    {
        return _mapper.Map<EnderecoViewModel>(await _enderecoRepository.ObterPorId(Id));

    }

    [ClaimsAuthorize("Fornecedor", "Atualizar")]
    [HttpPut("atualizar-endereco/{id:guid}")]
    public async Task<IActionResult> AtualizarEndereco(Guid id, EnderecoViewModel enderecoViewModel)
    {
        if (id != enderecoViewModel.Id) return BadRequest();

        if (!ModelState.IsValid) return CustomResponse(ModelState);

        await _fornecedorService.AtualizarEndereco(_mapper.Map<Endereco>(enderecoViewModel));

        return CustomResponse(enderecoViewModel);
    }

    private async Task<FornecedorViewModel> ObterFornecedorProdutosEndereco(Guid id)
    {
        return _mapper.Map<FornecedorViewModel>(await _fornecedorRepository.ObterFornecedorProdutosEndereco(id));
    }

    private async Task<FornecedorViewModel> ObterFornecedorEndereco(Guid id)
    {
        return _mapper.Map<FornecedorViewModel>(await _fornecedorRepository.ObterFornecedorEndereco(id));
    }

}
