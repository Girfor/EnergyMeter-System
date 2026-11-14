"""Replace the placeholder GitHub URL in Word reports, if needed.
Run: python Project/tools/fill_repository_link.py https://github.com/Girfor/EnergyMeter-System
"""
import sys
from pathlib import Path
from docx import Document

ROOT=Path(__file__).resolve().parents[2]
OLD='https://github.com/USERNAME/EnergyMeter-System'
if len(sys.argv)!=2 or not sys.argv[1].startswith('https://github.com/'):
    raise SystemExit('Usage: python fill_repository_link.py https://github.com/YOUR_USERNAME/EnergyMeter-System')
NEW=sys.argv[1].rstrip('/')
for path in sorted((ROOT/'Reports').glob('*.docx')):
    doc=Document(path)
    changed=0
    for p in doc.paragraphs:
        for run in p.runs:
            if OLD in run.text:
                run.text=run.text.replace(OLD,NEW);changed+=1
    for table in doc.tables:
        for row in table.rows:
            for cell in row.cells:
                for p in cell.paragraphs:
                    for run in p.runs:
                        if OLD in run.text:
                            run.text=run.text.replace(OLD,NEW); changed+=1
    if changed: doc.save(path)
    print(path.name,changed,'replacement(s)')
p=ROOT/'Project/REPOSITORY_URL.txt'
p.write_text('Репозиторий: '+NEW+'\n',encoding='utf8')
