# CLAUDE.md — capacete-vr

## Objetivo do Projeto
> **[A ser preenchido pelo usuário]**

---

## Contexto Técnico

### Engine e Plataforma
- **Engine**: Unity (Universal Render Pipeline — URP)
- **Target**: Headset VR com OpenXR (ex: Meta Quest)
- **Nome do produto**: `capacete` (`DefaultCompany`)
- **Versão**: `0.1.0`

### XR
- Loader: **OpenXR** (`Assets/XR/Loaders/OpenXRLoader.asset`)
- Input: `UnityEngine.XR` (`InputDevices`, `XRNode`, `CommonUsages`)
- Integração de mãos virtuais: **VirtualGrasp SDK** (Gleechi) — suporta Oculus/OVR, Unity XR, SteamVR, Leap Motion, Manus

---

## Arquitetura de Scripts

```
Assets/
├── _Scripts/
│   ├── CapaceteVR.asmdef           # Assembly de produção
│   ├── Input/
│   │   ├── IVRInput.cs             # Interface: contrato de leitura de input
│   │   └── VRControllerInput.cs    # Implementação real com XR SDK
│   └── Locomotion/
│       ├── GazeDirectionResolver.cs  # Lógica pura: calcula direção de movimento pelo olhar
│       ├── GazeLocomotion.cs         # MonoBehaviour: aplica movimento pelo CharacterController
│       └── SnapTurn.cs               # MonoBehaviour: rotação discreta por snap
│
├── _Tests/
│   ├── CapaceteVR.Tests.asmdef     # Assembly de testes (EditMode)
│   └── Locomotion/
│       ├── GazeDirectionResolverTests.cs
│       └── SnapTurnTests.cs
```

### Princípios adotados
- **Single Responsibility**: cada classe tem uma única razão para mudar
- **Dependency Inversion**: `GazeLocomotion` e `SnapTurn` dependem de `IVRInput`, nunca do hardware
- **Testabilidade**: lógica pura separada de `MonoBehaviour`; testes usam `FakeVRInput`
- **Sem gravidade**: o projeto não usa simulação de gravidade (confirmado pelo usuário)

---

## Navegação por Gaze (branch `feature/gaze-navigation`)

### Como funciona
1. `VRControllerInput` lê os analógicos do XR SDK e aplica deadzone
2. `GazeDirectionResolver.Resolve(head, thumbstick)` projeta `head.forward` e `head.right` no plano XZ e combina com o input
3. `GazeLocomotion` chama o resolver e move o `CharacterController`
4. `SnapTurn` rotaciona o corpo em ângulos discretos (padrão: 30°) via analógico direito

### Setup na cena
Adicione ao GameObject raiz do player:
- `CharacterController`
- `VRControllerInput`
- `GazeLocomotion` (arraste o Transform do HMD/câmera em `Head Transform`)
- `SnapTurn`

---

## Testes

- Framework: **NUnit** (Unity Test Runner — EditMode)
- Rodar: `Window > General > Test Runner > EditMode > Run All`
- `FakeVRInput` é a implementação de teste de `IVRInput`, definida em `SnapTurnTests.cs`

---

## Assets de Terceiros

| Asset | Local | Uso |
|-------|-------|-----|
| 3D Free Modular Kit (Barking Dog) | `Assets/Barking_Dog/` | Ambiente modular (paredes, pisos, portas) |
| VirtualGrasp SDK (Gleechi) | `Assets/com.gleechi.unity.virtualgrasp/` | Interação com mãos virtuais |

---

## Branches

| Branch | Propósito |
|--------|-----------|
| `master` | Código estável / base |
| `feature/gaze-navigation` | Locomotion por gaze + snap turn (trabalho atual) |

---

## Convenções

- Namespace raiz: `CapaceteVR`
- Sub-namespaces: `CapaceteVR.Input`, `CapaceteVR.Locomotion`, `CapaceteVR.Tests.*`
- Campos privados: prefixo `_camelCase`
- Sem gravidade artificial no `CharacterController`
- `#if UNITY_EDITOR` guarda helpers de injeção de dependência usados nos testes
