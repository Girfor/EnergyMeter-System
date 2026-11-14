"""Educational, reproducible estimate of maintainability index.
Not a replacement for Visual Studio Code Metrics / Roslyn analysis.
"""
import math, re
from pathlib import Path
ROOT=Path(__file__).resolve().parents[1]
PATHS={
    'before': ROOT/'examples/insecure/LegacyController.cs.txt',
    'after': ROOT/'src/EnergyMeter.Api/Domain/MeterReadingService.cs',
}
OPERATORS=set('if return new null var && || == != <= >= < > = ? : . + - * /'.split())
def extract_method(source, signature):
    start=source.index(signature)
    opened=source.index('{', start)
    depth=0
    for i in range(opened,len(source)):
        if source[i]=='{': depth+=1
        elif source[i]=='}':
            depth-=1
            if depth==0: return source[start:i+1]
    raise ValueError('Could not find method end')

def analyze(path, cc, signature):
    code=extract_method(path.read_text(encoding='utf8'),signature)
    code=re.sub(r'//[^\n]*','',code)
    loc=sum(bool(s.strip()) for s in code.splitlines())
    tokens=re.findall(r'"[^"]*"|[A-Za-z_][\w]*|\d+(?:\.\d+)?|==|!=|<=|>=|&&|\|\||=>|\?\?|[^\s]',code)
    ops=[t for t in tokens if t in OPERATORS or t in '(){}[],;']
    operands=[t for t in tokens if t not in OPERATORS and t not in '(){}[],;']
    N=len(ops)+len(operands); n=len(set(ops))+len(set(operands)); V=N*math.log2(max(n,2))
    mi=max(0,(171-5.2*math.log(max(V,1))-0.23*cc-16.2*math.log(max(loc,1)))*100/171)
    return dict(nonempty_loc=loc, token_volume=round(V,1), estimated_MI=round(mi,1), cc_assumed=cc)
if __name__=='__main__':
    for key,path in PATHS.items(): print(key,path.name,analyze(path,8 if key=='before' else 3, 'public IActionResult Save' if key=='before' else 'public CreateReadingResult Create'))
