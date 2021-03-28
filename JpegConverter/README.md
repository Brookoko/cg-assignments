# JPG/JPEG converter module 
## Description

## References: 
### Specifications and documentation:
- CCIT recommendation T.81: Infromation technology - digital compression and coding of 
continuous-tone still images - requirements and guidelines: [Recommendation document link.](https://www.w3.org/Graphics/JPEG/itu-t81.pdf)\
All the names, used in this module, are specified by this recommendation.

## Capabilities: 
### Implementation checklist
- Classes: 
  - [x] Lossy
  - [ ] Lossless
- Modes: 
  - [x] Sequential DCT-based coding mode
  - [ ] Progressive DCT-based coding mode
  - [ ] Hierararchical mode
  - [ ] Lossless mode
- Sample precisions: 
  - [x] 8-bits samples coding
  - [ ] 12-bits samples coding
- Coding encoder/decoders:
  - Huffman:
    - [x] Encoder 
    - [ ] Decoder
  - Arithmetic: 
    - [ ] Encoder
    - [ ] Decoder  
  
### Other usages list:
- Data unit encoding order:
  - [x] Non-interleaved 
  - [ ] Interleaved 
- Quantized coefficients encoding procedures:
  - [x] Spectral selection
  - [ ] Successive approximation
